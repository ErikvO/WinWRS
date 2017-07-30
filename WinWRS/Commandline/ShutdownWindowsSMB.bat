@echo off
set Computer=%1
set Username=%2
set Password=%3
set ShutdownParameters=%~4
net use \\%Computer%\IPC$ %Password% /USER:%Username%
shutdown %ShutdownParameters% /t 0 /m \\%Computer%
