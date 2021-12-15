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
| Retrieve the job queue status       | No    | No      |
| Enqueue a job                       | No    | No      |
| Remove a Job                        | No    | No      |
| Pause the job queue                 | No    | No      |
| Start the job queue                 | No    | No      |

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
| Get Device List                     | No    | No      |
| Get Device Status                   | No    | No      |
| Set Device State                    | No    | No      |
| Get Batch Device Status             | No    | No      |
| Batch Power On Devices              | No    | No      |
| Batch Power Off Devices             | No    | No      |

## Octoprint API emulation

| Function                               | Added?| Tested? |
| ---------------------------------------|:-----:| -------:|
| Version information                    | No    | No      |
| Server status                          | No    | No      |
| Login verification & User information  | No    | No      |
| Get settings                           | No    | No      |
| Octoprint File Upload                  | No    | No      |
| Get Job status                         | No    | No      |
| Get Printer status                     | No    | No      |
| Send GCode command                     | No    | No      |
| List Printer profiles                  | No    | No      |

## History APIs

| Function                               | Added?| Tested? |
| ---------------------------------------|:-----:| -------:|
| Get job list                           | No    | No      |
| Get job totals                         | No    | No      |
| Reset totals                           | No    | No      |
| Get a single job                       | No    | No      |
| Delete job                             | No    | No      |

## MQTT APIs

| Function                               | Added?| Tested? |
| ---------------------------------------|:-----:| -------:|
| Publish a topic                        | No    | No      |
| Subscribe to a topic                   | No    | No      |

## Websocket notifications
Not implemented yet.
