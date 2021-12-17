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
| Emergency Stop                      | ✅   | ✅      |
| Host Restart                        | ✅   | ✅      |
| Firmware Restart                    | ✅   | ✅      |

## Printer Status

| Function                            | Added?| Tested? |
| ----------------------------------- |:-----:| -------:|
| List available printer objects      | ✅   | ✅      |
| Query printer object status         | ✅   | ✅      |
| Subscribe to printer object status  | ✅   | No      |
| Query Endstops                      | ✅   | ✅      |
| Query Server Info                   | ✅   | ✅      |
| Get Server Configuration            | ✅   | ✅      |
| Request Cached Temperature Data     | ✅   | ✅      |
| Request Cached GCode Responses      | ✅   | ✅      |
| Restart Server                      | ✅   | ✅      |
| Get Websocket ID                    | n.a.  | n.a.    |

## GCode APIs

| Function                            | Added?| Tested? |
| ----------------------------------- |:-----:| -------:|
| Run a gcode                         | ✅   | ✅      |
| Get GCode Help                      | ✅   | ✅      |

## Print Management

| Function                            | Added?| Tested? |
| ----------------------------------- |:-----:| -------:|
| Print a file                        | ✅   | ✅      |
| Pause a print                       | ✅   | ✅      |
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
| List available files                | ✅   | ✅      |
| Get gcode metadata                  | ✅   | ✅      |
| Get directory information           | ✅   | ✅      |
| Create directory                    | ✅   | ✅      |
| Delete directory                    | ✅   | ✅      |
| Move a file or directory            | ✅   | ✅      |
| Copy a file or directory            | ✅   | ✅      |
| File download                       | ✅   | No      |
| File upload                         | ✅   | No      |
| File delete                         | ✅   | ✅      |
| Download klippy.log                 | ✅   | ✅      |
| Download moonraker.log              | ✅   | ✅      |


## Authorization

| Function                            | Added?| Tested? |
| ----------------------------------- |:-----:| -------:|
| Login User                          | ✅   | ✅      |
| Logout Current User                 | ✅   | ✅      |
| Get Current User                    | ✅   | ✅      |
| Create User                         | ✅   | ✅      |
| Delete User                         | ✅   | ✅      |
| List Available Users                | ✅   | ✅      |
| Reset User Password                 | ✅   | ✅      |
| Refresh JSON Web Token              | ✅   | ✅      |
| Generate a Oneshot Token            | ✅   | ✅      |
| Get the Current API Key             | ✅   | ✅      |
| Generate a New API Key              | ✅   | ✅      |

## Database APIs
Implementation not planned at the moment.

## Job Queue APIs

| Function                            | Added?| Tested? |
| ----------------------------------- |:-----:| -------:|
| Retrieve the job queue status       | ✅   | ✅      |
| Enqueue a job                       | ✅   | ✅      |
| Remove a Job                        | ✅   | ✅      |
| Pause the job queue                 | ✅   | ✅      |
| Start the job queue                 | ✅   | ✅      |

## Update Manager APIs

| Function                            | Added?| Tested? |
| ----------------------------------- |:-----:| -------:|
| Get update status                   | ✅    | No      |
| Perform a full update               | ✅    | No      |
| Update Moonraker                    | ✅    | No      |
| Update Klipper                      | ✅    | No      |
| Update Client                       | ✅    | No      |
| Update System Packages              | ✅    | No      |
| Recover a corrupt repo              | ✅    | No      |

## Power APIs

| Function                            | Added?| Tested? |
| ----------------------------------- |:-----:| -------:|
| Get Device List                     | ✅    | ✅     |
| Get Device Status                   | ✅    | ✅     |
| Set Device State                    | ✅    | ✅     |
| Get Batch Device Status             | ✅    | ✅     |
| Batch Power On Devices              | ✅    | ✅     |
| Batch Power Off Devices             | ✅    | ✅     |

## Octoprint API emulation

| Function                               | Added?| Tested? |
| ---------------------------------------|:-----:| -------:|
| Version information                    | ✅    | ✅     |
| Server status                          | ✅    | ✅     |
| Login verification & User information  | ✅    | No     |
| Get settings                           | ✅    | ✅     |
| Octoprint File Upload                  | ✅    | ✅     |
| Get Job status                         | ✅    | ✅     |
| Get Printer status                     | ✅    | ✅     |
| Send GCode command                     | ✅    | ✅     |
| List Printer profiles                  | ✅    | ✅     |

## History APIs

| Function                               | Added?| Tested? |
| ---------------------------------------|:-----:| -------:|
| Get job list                           | ✅    | ✅     |
| Get job totals                         | ✅    | ✅     |
| Reset totals                           | ✅    | ✅     |
| Get a single job                       | ✅    | ✅     |
| Delete job                             | ✅    | ✅     |

## MQTT APIs

| Function                               | Added?| Tested? |
| ---------------------------------------|:-----:| -------:|
| Publish a topic                        | No    | No      |
| Subscribe to a topic                   | No    | No      |

## Websocket notifications
Not implemented yet.
