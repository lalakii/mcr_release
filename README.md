# mcr_release
Update the microcode in your bios firmware with this tool.

You can use [MCExtractor](https://github.com/platomav/MCExtractor) to view and extract the current microcode.

You can download the microcode from [CPUMicrocodes](https://github.com/platomav/CPUMicrocodes).

Linux:

```console
chmod +x mcr.linux-x64

./mcr.linux-x64 -u "your_bios_file" -src "your_current_microcode" -dest "new_microcode"
```
Windows Powershell:
```console
.\mcr -u "your_bios_file" -src "your_current_microcode" -dest "new_microcode"
```
Windows CMD:
```console
mcr -u "your_bios_file" -src "your_current_microcode" -dest "new_microcode"
```


&copy; 2024 lalaki.cn
