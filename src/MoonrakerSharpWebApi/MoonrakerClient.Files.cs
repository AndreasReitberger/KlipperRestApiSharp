using AndreasReitberger.API.Moonraker.Enum;
using AndreasReitberger.API.Moonraker.Models;
using AndreasReitberger.API.Moonraker.Structs;
using AndreasReitberger.API.Print3dServer.Core.Events;
using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using AndreasReitberger.API.REST.Events;
using AndreasReitberger.API.REST.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AndreasReitberger.API.Moonraker
{
    public partial class MoonrakerClient
    {
        #region Properties

        /*
        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        ObservableCollection<KlipperFile> files = new();
        partial void OnFilesChanged(ObservableCollection<KlipperFile> value)
        {
            OnKlipperFilesChanged(new KlipperFilesChangedEventArgs()
            {
                NewFiles = value,
                SessonId = SessionId,
                CallbackId = -1,
                Token = !string.IsNullOrEmpty(UserToken) ? UserToken : ApiKey,
            });
        }
        */

        [ObservableProperty]
        [property: JsonIgnore, System.Text.Json.Serialization.JsonIgnore, XmlIgnore]
        ObservableCollection<KlipperDirectory> availableDirectories = [];
        partial void OnAvailableDirectoriesChanged(ObservableCollection<KlipperDirectory> value)
        {
            /*
            OnKlipperFilesChanged(new KlipperFilesChangedEventArgs()
            {
                NewFiles = value,
                SessonId = SessionId,
                CallbackId = -1,
                Token = !string.IsNullOrEmpty(UserToken) ? UserToken : API,
            });
            */
        }

        #endregion

        #region Methods

        // Doc: https://github.com/Arksine/moonraker/blob/master/docs/web_api.md#list-available-files
        public async Task RefreshAvailableFilesAsync(string rootPath = "", bool includeGcodeMeta = true)
        {
            try
            {
                List<IGcode> files = await GetAvailableFilesAsync(rootPath, includeGcodeMeta).ConfigureAwait(false);
                Files = [.. files];
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                Files = [];
            }
        }

        public async Task<List<IGcode>> GetAvailableFilesAsync(string rootPath = "", bool includeGcodeMeta = true)
        {
            IRestApiRequestRespone? result = null;
            List<IGcode> resultObject = [];
            try
            {
                Dictionary<string, string> urlSegments = [];
                if (!string.IsNullOrEmpty(rootPath))
                {
                    urlSegments.Add("rquestTargetUri", rootPath);
                }

                string targetUri = $"{MoonrakerCommands.Server}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: "files/list",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                //result = await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Get, "files/list", default, null, urlSegments).ConfigureAwait(false);
                KlipperFileListRespone? files = GetObjectFromJson<KlipperFileListRespone>(result?.Result, NewtonsoftJsonSerializerSettings);
                if (includeGcodeMeta)
                {
                    for (int i = 0; i < files?.Result?.Count; i++)
                    {
                        using KlipperFile? current = files?.Result[i];
                        if (current is not null)
                        {
                            current.Meta = await GetGcodeMetadataAsync(current.FilePath).ConfigureAwait(false);
                            if (current.Meta?.GcodeImages?.Count > 0)
                            {
                                current.Image = await GetGcodeSecondThumbnailImageAsync(current.Meta)
                                    .ConfigureAwait(false)
                                    ;
                            }
                        }
                    }
                }
                return [.. files?.Result];
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
                    TargetType = nameof(KlipperFileListRespone),
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        public Task<List<IGcode>> GetAvailableFilesAsListAsync(string rootPath = "") 
            => GetAvailableFilesAsync(rootPath);

        public override Task<List<IGcode>> GetFilesAsync() => GetAvailableFilesAsync();

        public async Task<KlipperGcodeMetaResult?> GetGcodeMetadataAsync(string fileName)
        {
            IRestApiRequestRespone? result = null;
            KlipperGcodeMetaResult? resultObject = null;
            try
            {
                if (string.IsNullOrEmpty(fileName)) return resultObject;

                Dictionary<string, string> urlSegments = new()
                {
                    { "filename", fileName }
                };

                string targetUri = $"{MoonrakerCommands.Server}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: "files/metadata",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                //result = await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Get, "files/metadata", default, null, urlSegments).ConfigureAwait(false);
                KlipperGcodeMetaRespone? queryResult = GetObjectFromJson<KlipperGcodeMetaRespone>(result?.Result, NewtonsoftJsonSerializerSettings);
                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
                    TargetType = nameof(KlipperGcodeMetaRespone),
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        public async Task RefreshGcodeMetadataAsync(string fileName)
        {
            try
            {
                KlipperGcodeMetaResult? meta = await GetGcodeMetadataAsync(fileName).ConfigureAwait(false);
                GcodeMeta = meta;
                if (PrintStats?.State == KlipperPrintStates.Printing)
                {
                    // Get current print image 
                    CurrentPrintImage = await GetGcodeLargestThumbnailImageAsync(GcodeMeta) ?? [];
                }
                else
                {
                    CurrentPrintImage = [];
                }
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                GcodeMeta = null;
            }
        }

        public async Task<byte[]> GetGcodeThumbnailImageAsync(string relativePath, int timeout = 10000)
        {
            try
            {
                string target = $"{FullWebAddress}/server/files/gcodes/{relativePath}";
                byte[]? thumb = await DownloadFileFromUriAsync(target, timeout)
                    .ConfigureAwait(false);
                return thumb ?? [];
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return [];
            }
        }
        public async Task<byte[]?> GetGcodeThumbnailImageAsync(IGcodeMeta? gcodeMeta, int index = 0, int timeout = 10000)
        {
            if (gcodeMeta is null || gcodeMeta?.GcodeImages is null) return [];
            string? path = gcodeMeta.GcodeImages.Count > index ?
                gcodeMeta.GcodeImages[index]?.Path : gcodeMeta.GcodeImages.FirstOrDefault()?.Path;

            string subfolder = string.Empty;
            if (gcodeMeta?.FileName?.Contains("/") ?? false)
            {
                subfolder = gcodeMeta.FileName[..gcodeMeta.FileName.LastIndexOf("/")];
                subfolder += "/";
            }
            return string.IsNullOrEmpty(path) ? null : await GetGcodeThumbnailImageAsync(subfolder + path, timeout)
                .ConfigureAwait(false);
        }
        public async Task<byte[]?> GetGcodeLargestThumbnailImageAsync(KlipperGcodeMetaResult? gcodeMeta, int timeout = 10000)
        {
            if (gcodeMeta is null || gcodeMeta?.GcodeImages is null) return [];
            string? path = gcodeMeta.GcodeImages
                .OrderByDescending(image => image.Size)
                .FirstOrDefault()?.Path
                ;

            string subfolder = string.Empty;
            if (gcodeMeta?.FileName?.Contains("/") ?? false)
            {
                subfolder = gcodeMeta.FileName[..gcodeMeta.FileName.LastIndexOf("/")];
                subfolder += "/";
            }

            return string.IsNullOrEmpty(path) ? null : await GetGcodeThumbnailImageAsync(subfolder + path, timeout)
                .ConfigureAwait(false)
                ;
        }
        public async Task<byte[]?> GetGcodeSmallestThumbnailImageAsync(KlipperGcodeMetaResult? gcodeMeta, int timeout = 10000)
        {
            if (gcodeMeta is null || gcodeMeta?.GcodeImages is null) return [];
            string? path = gcodeMeta.GcodeImages
                .OrderBy(image => image.Size)
                .FirstOrDefault()?.Path
                ;

            string subfolder = string.Empty;
            if (gcodeMeta?.FileName?.Contains("/") ?? false)
            {
                //subfolder = gcodeMeta.Filename.Substring(0, gcodeMeta.Filename.LastIndexOf("/"));
                subfolder = gcodeMeta.FileName[..gcodeMeta.FileName.LastIndexOf("/")];
                subfolder += "/";
            }

            return string.IsNullOrEmpty(path) ? null : await GetGcodeThumbnailImageAsync(subfolder + path, timeout)
                .ConfigureAwait(false)
                ;
        }
        public async Task<byte[]?> GetGcodeSecondThumbnailImageAsync(IGcodeMeta? gcodeMeta, int timeout = 10000)
        {
            if (gcodeMeta is null || gcodeMeta?.GcodeImages is null) return [];
            string? path = gcodeMeta.GcodeImages
                .OrderBy(image => image.Size)?
                .Skip(1)? // Skipped the smallest image
                .FirstOrDefault()?.Path
                ;

            string subfolder = string.Empty;
            if (gcodeMeta?.FileName?.Contains("/") ?? false)
            {
                subfolder = gcodeMeta.FileName[..gcodeMeta.FileName.LastIndexOf("/")];
                subfolder += "/";
            }

            return string.IsNullOrEmpty(path) ? null : await GetGcodeThumbnailImageAsync(subfolder + path, timeout)
                .ConfigureAwait(false)
                ;
        }
        public async Task RefreshDirectoryInformationAsync(string path = "", bool extended = true)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                    path = "gcodes";
                KlipperDirectoryInfoResult? result = await GetDirectoryInformationAsync(path, extended).ConfigureAwait(false);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }

        public async Task<KlipperDirectoryInfoResult?> GetDirectoryInformationAsync(string path, bool extended = true)
        {
            IRestApiRequestRespone? result = null;
            KlipperDirectoryInfoResult? resultObject = null;
            try
            {
                Dictionary<string, string> urlSegments = new()
                {
                    { "path", path },
                    { "extended", extended ? "true" : "false" }
                };

                string targetUri = $"{MoonrakerCommands.Server}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: "files/directory",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                //result = await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Get, "files/directory", default, null, urlSegments).ConfigureAwait(false);
                KlipperDirectoryInfoRespone? queryResult = GetObjectFromJson<KlipperDirectoryInfoRespone>(result?.Result, NewtonsoftJsonSerializerSettings);
                if (queryResult?.Result?.DiskUsage is not null)
                {
                    FreeDiskSpace = queryResult.Result.DiskUsage.Free;
                    TotalDiskSpace = queryResult.Result.DiskUsage.Total;
                    UsedDiskSpace = queryResult.Result.DiskUsage.Used;
                }
                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
                    TargetType = nameof(KlipperDirectoryInfoRespone),
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public override async Task<List<IGcodeGroup>> GetModelGroupsAsync(string path = "")
        {
            List<KlipperDirectory> directories = await GetAvailableDirectoriesAsync(path);
            return [.. directories];
        }

        public async Task<List<KlipperDirectory>> GetAvailableDirectoriesAsync(string path = "")
        {
            List<KlipperDirectory> resultObject = [];
            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    path = "gcodes";
                }
                KlipperDirectoryInfoResult? result = await GetDirectoryInformationAsync(path, false).ConfigureAwait(false);
                return [.. result?.Dirs];
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task RefreshAvailableDirectorienAsync(string path = "")
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    path = "gcodes";
                }
                List<KlipperDirectory> result = await GetAvailableDirectoriesAsync(path).ConfigureAwait(false);
                AvailableDirectories = new(result);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }

        public async Task<KlipperDirectoryActionResult?> CreateDirectoryAsync(string directory)
        {
            IRestApiRequestRespone? result = null;
            KlipperDirectoryActionResult? resultObject = null;
            try
            {
                Dictionary<string, string> urlSegments = new()
                {
                    { "path", directory }
                };

                string targetUri = $"{MoonrakerCommands.Server}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: "files/directory",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Post, $"files/directory", jsonObject: null, cts: default, urlSegments: urlSegments)
                    .ConfigureAwait(false);
                */
                KlipperDirectoryActionRespone? queryResult = GetObjectFromJson<KlipperDirectoryActionRespone>(result?.Result, NewtonsoftJsonSerializerSettings);
                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
                    TargetType = nameof(KlipperDirectoryActionRespone),
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<KlipperDirectoryActionResult?> DeleteDirectoryAsync(string directory, bool force = false)
        {
            IRestApiRequestRespone? result = null;
            KlipperDirectoryActionResult? resultObject = null;
            try
            {
                Dictionary<string, string> urlSegments = new()
                {
                    { "path", directory },
                    { "force", force ? "true" : "false" }
                };

                string targetUri = $"{MoonrakerCommands.Server}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Delete,
                       command: "files/directory",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Delete, $"files/directory", jsonObject: null, cts: default, urlSegments: urlSegments)
                    .ConfigureAwait(false);
                */
                KlipperDirectoryActionRespone? queryResult = GetObjectFromJson<KlipperDirectoryActionRespone>(result?.Result, NewtonsoftJsonSerializerSettings);
                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
                    TargetType = nameof(KlipperDirectoryActionRespone),
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<KlipperDirectoryActionResult?> MoveDirectoryOrFileAsync(string source, string destination)
        {
            IRestApiRequestRespone? result = null;
            KlipperDirectoryActionResult? resultObject = null;
            try
            {
                Dictionary<string, string> urlSegments = new()
                {
                    { "source", source },
                    { "dest", destination }
                };

                string targetUri = $"{MoonrakerCommands.Server}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: "files/move",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Post, $"files/move", jsonObject: null, cts: default, urlSegments: urlSegments)
                    .ConfigureAwait(false);
                */
                KlipperDirectoryActionRespone? queryResult = GetObjectFromJson<KlipperDirectoryActionRespone>(result?.Result, NewtonsoftJsonSerializerSettings);
                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
                    TargetType = nameof(KlipperDirectoryActionRespone),
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<KlipperDirectoryActionResult?> CopyDirectoryOrFileAsync(string source, string destination)
        {
            IRestApiRequestRespone? result = null;
            KlipperDirectoryActionResult? resultObject = null;
            try
            {
                Dictionary<string, string> urlSegments = new()
                {
                    { "source", source },
                    { "dest", destination }
                };

                string targetUri = $"{MoonrakerCommands.Server}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: "files/copy",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Post, $"files/copy", jsonObject: null, cts: default, urlSegments: urlSegments)
                    .ConfigureAwait(false);
                */
                KlipperDirectoryActionRespone? queryResult = GetObjectFromJson<KlipperDirectoryActionRespone>(result?.Result, NewtonsoftJsonSerializerSettings);
                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
                    TargetType = nameof(KlipperDirectoryActionRespone),
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public override async Task<byte[]?> DownloadFileAsync(string relativeFilePath)
        {
            try
            {
                //Uri uri = new($"{FullWebAddress}/server/files/{relativeFilePath}");
                string uri = $"{FullWebAddress}/server/files/{relativeFilePath}";
                byte[]? file = await DownloadFileFromUriAsync(uri)
                    .ConfigureAwait(false)
                    ;
                return file;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return null;
            }
        }

        public async Task<KlipperFileActionResult?> UploadFileAsync(string localFilePath, string root = "gcodes", string path = "", int timeout = 100000)
        {
            IRestApiRequestRespone? result = null;
            KlipperFileActionResult? resultObject = null;
            try
            {
                string targetFilePath = $"/server/files/upload";
                FileInfo info = new(localFilePath);
                Dictionary<string, string>? parameters = new()
                {
                    { "rquestTargetUri", root },
                    { "path", "" }
                };

                result = await SendMultipartFormDataFileRestApiRequestAsync(
                    fileName: info.Name, file: null, localFilePath: localFilePath, requestTargetUri: targetFilePath, 
                    parameters: parameters,
                    authHeaders: AuthHeaders, timeout: timeout
                    )
                    .ConfigureAwait(false);
                
                KlipperFileActionResult? queryResult = GetObjectFromJson<KlipperFileActionResult>(result?.Result, NewtonsoftJsonSerializerSettings);
                return queryResult;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
                    TargetType = nameof(KlipperFileActionResult),
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return null;
            }
        }

        public async Task<KlipperFileActionResult?> UploadFileAsync(string fileName, byte[] file, string root = "gcodes", string path = "", int timeout = 100000)
        {
            IRestApiRequestRespone? result = null;
            KlipperFileActionResult? resultObject = null;
            try
            {
                string targetFilePath = $"/server/files/upload";
                Dictionary<string, string>? parameters = new()
                {
                    { "root", root },
                    { "path", "" }
                };
                result = await SendMultipartFormDataFileRestApiRequestAsync(
                    fileName: fileName, file: file, requestTargetUri: targetFilePath, authHeaders: AuthHeaders, timeout: timeout, parameters: parameters)
                    .ConfigureAwait(false);
                KlipperFileActionResult? queryResult = GetObjectFromJson<KlipperFileActionResult>(result?.Result, NewtonsoftJsonSerializerSettings);
                return queryResult;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
                    TargetType = nameof(KlipperFileActionResult),
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return null;
            }
        }

        public async Task<KlipperDirectoryActionResult?> DeleteFileAsync(string root, string filePath)
        {
            IRestApiRequestRespone? result = null;
            KlipperDirectoryActionResult? resultObject = null;
            try
            {
                string targetUri = $"{MoonrakerCommands.Server}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Delete,
                       command: $"files/{root}/{filePath}",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Delete, $"files/{rquestTargetUri}/{filePath}")
                    .ConfigureAwait(false);
                */
                KlipperDirectoryActionRespone? queryResult = GetObjectFromJson<KlipperDirectoryActionRespone>(result?.Result, NewtonsoftJsonSerializerSettings);
                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
                    TargetType = nameof(KlipperDirectoryActionResult),
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        public async Task<KlipperDirectoryActionResult?> DeleteFileAsync(string filePath)
        {
            IRestApiRequestRespone? result = null;
            KlipperDirectoryActionResult? resultObject = null;
            if (string.IsNullOrEmpty(filePath))
            {
                return resultObject;
            }
            try
            {
                string targetUri = $"{MoonrakerCommands.Server}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Delete,
                       command: $"files/{filePath}",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                result =
                    await SendRestApiRequestAsync(MoonrakerCommandBase.server, Method.Delete, $"files/{filePath}")
                    .ConfigureAwait(false);
                */
                KlipperDirectoryActionRespone? queryResult = GetObjectFromJson<KlipperDirectoryActionRespone>(result?.Result, NewtonsoftJsonSerializerSettings);
                return queryResult?.Result;
            }
            catch (JsonException jecx)
            {
                OnError(new JsonConvertEventArgs()
                {
                    Exception = jecx,
                    OriginalString = result?.Result,
                    TargetType = nameof(KlipperDirectoryActionRespone),
                    Message = jecx.Message,
                });
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<byte[]?> DownloadLogFileAsync(KlipperLogFileTypes logType)
        {
            try
            {
                string uri = $"{FullWebAddress}/server/files/{logType.ToString().ToLower()}.log";
                byte[]? file = await DownloadFileFromUriAsync(uri)
                    .ConfigureAwait(false);
                return file;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return null;
            }
        }
        #endregion
    }
}
