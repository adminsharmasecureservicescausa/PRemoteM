# PRemoteM.

English | [中文](https://github.com/OneRemote/PRemoteM/wiki/Intro-%E7%AE%80%E4%BD%93%E4%B8%AD%E6%96%87)

[![version](https://img.shields.io/github/v/release/vshawn/premotem?color=Green&include_prereleases)](https://github.com/OneRemote/PRemoteM/releases)
[![codebeat badge](https://codebeat.co/badges/c3e71ab7-bf2c-455b-9886-d5956723eed5)](https://codebeat.co/projects/github-com-vshawn-premotem-dev)
[![issues](https://img.shields.io/github/issues/vshawn/premotem)](https://github.com/OneRemote/PRemoteM/issues)
[![license](https://img.shields.io/github/license/vshawn/premotem?color=blue)](https://github.com/OneRemote/PRemoteM/blob/dev/LICENSE)
[![Build status](https://ci.appveyor.com/api/projects/status/2qbmeeurdq0vijmf/branch/dev?svg=true)](https://ci.appveyor.com/project/VShawn/premotem-build-dev/branch/dev)
![Hits](https://hits.seeyoufarm.com/api/count/incr/badge.svg?url=https%3A%2F%2Fgithub.com%2Fvshawn%2Fpremotem&count_bg=%23E83D61&title_bg=%23102B3E&icon=github.svg&icon_color=%23CED8E1&title=&edge_flat=false)

PRemoteM is a modern personal remote session manager and launcher. It is a single place to manage all your remote sessions supporting number of different protocols.

## Features

- Supports RDP, SSH, VNC, Telnet, (S)FTP, [RemoteApp](https://github.com/OneRemote/PRemoteM/wiki/%5BProtocol%5D-RemoteApp), [NoMachine and other app](https://github.com/OneRemote/PRemoteM/wiki/%5BProtocol%5D-APP-NoMachine)
- Quick and convenient remote session launcher (Alt + M)
- Multi-screen and HiDPI RDP connection (Test on **Win10 + 4k monitor *2** RDP TO **Win2016**)
- Detailed connection configuration: tags, icons, colors, connection scripts etc.
- Multiple languages, themes and tabbed interface
- [Import connections from mRemoteNG](https://raw.githubusercontent.com/VShawn/PRemoteM/Doc/DocPic/Migrate.jpg)
- [Password encryption via RSA](https://github.com/OneRemote/PRemoteM/wiki/Security)
- Customizable runners, in SFTP \ FTP \ VNC \ etc. protocols, you can replace the internal runner with your favourite tools.[wiki](https://github.com/OneRemote/PRemoteM/wiki/%5BProtocol%5D--Protocol-Runners)
- Portable - just unpack and run

## Installation

Latest Version: 0.6.2.2

Pre-Release Build: [0.7.0-alpha](https://github.com/1Remote/PRemoteM/releases/tag/0.7.0-pre-release-2)

Use one of the following methods to install the application:

- [GitHub release](https://github.com/OneRemote/PRemoteM/releases)
- [Microsoft Store](https://www.microsoft.com/store/productId/9PNMNF92JNFP)
- [Chocolatey](https://chocolatey.org/packages/premotem): `choco install premotem`

### Requirements

- [Windows10 17763 and above](https://support.lenovo.com/us/en/solutions/ht502786)
- [.NET Framework 4.8](https://dotnet.microsoft.com/download/dotnet-framework/net48)

P.S. You can clone the code and remove all of the Win10 dependencies if you are likely to use PRemoteM in Win7.

### [Quick start](https://github.com/OneRemote/PRemoteM/wiki/Quick-start)

1. Open PRemote.exe.

2. Click "+" button and fill address\username\password... then save

3. Press <kbd>Alt</kbd> + <kbd>M</kbd> Open the launcher, type keyword to find your server, press <kbd>enter</kbd> to start session

## Overview

<img src="https://raw.githubusercontent.com/VShawn/PRemoteM/Doc/DocPic/maindemo.png" width="800" />


<p align="center">
    <img src="https://raw.githubusercontent.com/VShawn/PRemoteM/Doc/DocPic/quickstart.gif" width="400"/>
</p>

<p align="center">
    ↑ Launcher(Alt + M) open RDP connection & resizing
</p>


<p align="center">
    <img src="https://raw.githubusercontent.com/VShawn/PRemoteM/Doc/DocPic/tab.gif" width="500" />
/p>

<p align="center">
    ↑ Tab detach & SSH auto command after connected
</p>

<p align="center">
    <img src="https://raw.githubusercontent.com/VShawn/PRemoteM/Doc/DocPic/multi-screen.jpg" width="500"/>
</p>


<p align="center">
    ↑ RDP with Multi-monitors
</p>

<p align="center">
    <img src="https://raw.githubusercontent.com/VShawn/PRemoteM/Doc/DocPic/RemoteApp/demo.jpg" width="800"/>
</p>

<p align="center">
    ↑ RemoteApp via RDP
</p>

<p align="center">
    <img src="https://raw.githubusercontent.com/VShawn/PRemoteM/Doc/DocPic/Runner/vnc_runners.jpg" width="800"/>
</p>

<p align="center">
    ↑ Customizable runners
</p>

## Make PRemoteM Stronger

If you like **PRemoteM**, help us make it stronger by doing any of the following:

1. Simply star this repository
2. [Help translation](https://github.com/OneRemote/PRemoteM/wiki/Help-wanted:-Translation)
3. [Buy a coffee](https://ko-fi.com/VShawn)
4. [Join DEV](DEVELOP.md)

## Special thanks

<a href="http://www.jetbrains.com/resharper/"><img src="http://www.tom-englert.de/Images/icon_ReSharper.png" alt="ReSharper" width="64" height="64" /></a>

