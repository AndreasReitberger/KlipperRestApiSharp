# KlipperRestApiSharp
A C# based library to communicate with a klipper using MainsailOS.

# Moonrakers Documentation
This C# library wraps the available Web API functions listed in the
Moonraker's documentation below. 
https://moonraker.readthedocs.io/en/latest/web_api/

You can see the current migration progress in the following table.

# WebAPI migration status
Following you'll find the list of migrated functions from the WebAPI.

## Printer Administration

| Function                            | Added?| Tested? |
| ----------------------------------- |:-----:| -------:|
| Get Klippy host information         | ✅   | ✅      |
| Emergency Stop                      | ✅   | No      |
| Host Restart                        | ✅   | ✅      |
| Firmware Restart                    | ✅   | ✅      |

## Printer Status

| Function                            | Added?| Tested? |
| ----------------------------------- |:-----:| -------:|
| List available printer objects      | ✅   | ✅      |
| Query printer object status         | ✅   | ✅      |
| Subscribe to printer object status  | No    | No      |
| Query Endstops                      | No    | No      |
| Query Server Info                   | ✅   | ✅      |
| Get Server Configuration            | ✅   | ✅      |
| Request Cached Temperature Data     | ✅   | ✅      |
| Request Cached GCode Responses      | ✅   | ✅      |
| Restart Server                      | ✅   | ✅      |
| Get Websocket ID                    | n.a.  | n.a.    |

## GCode APIs

| Function                            | Added?| Tested? |
| ----------------------------------- |:-----:| -------:|
| Run a gcode                         | ✅   | No      |
| Get GCode Help                      | ✅   | ✅      |

## Print Management

| Function                            | Added?| Tested? |
| ----------------------------------- |:-----:| -------:|
| Print a file                        | ✅   | No      |
| Pause a print                       | ✅   | No      |
| Resume a print                      | ✅   | No      |
| Cancel a print                      | ✅   | No      |

## Machine Commands

| Function                            | Added?| Tested? |
| ----------------------------------- |:-----:| -------:|
| Get System Info                     | ✅   | ✅      |
| Shutdown the Operating System       | ✅   | ✅      |
| Reboot the Operating System         | ✅   | ✅      |
| Restart a system service            | ✅   | No      |
| Stop a system service               | ✅   | No      |
| Start a system service              | ✅   | No      |
| Get Moonraker Process Stats         | ✅   | ✅      |

## File Operations

| Function                            | Added?| Tested? |
| ----------------------------------- |:-----:| -------:|
| List available files                | ✅   | No      |
| Get gcode metadata                  | No   | No      |
| Get directory information           | No   | No      |
| Create directory                    | No   | No      |
| Delete directory                    | No   | No      |
| Move a file or directory            | No   | No      |
| Copy a file or directory            | No   | No      |
| File download                       | No   | No      |
| File upload                         | No   | No      |
| File delete                         | No   | No      |
| Download klippy.log                 | No   | No      |
| Download moonraker.log              | No   | No      |


## Authorization

| Function                            | Added?| Tested? |
| ----------------------------------- |:-----:| -------:|
| Login User                          | No   | No      |
| Logout Current User                 | No   | No      |
| Get Current User                    | No   | No      |
| Create User                         | No   | No      |
| Delete User                         | No   | No      |
| List Available Users                | No   | No      |
| Reset User Password                 | No   | No      |
| Refresh JSON Web Token              | No   | No      |
| Generate a Oneshot Token            | ✅   | ✅      |
| Get the Current API Key             | ✅   | ✅      |
| Generate a New API Key              | ✅   | ✅      |

