# SDRSharp.SDRplay-API3 RSP1 RSP1A
SDRSharp.SDRplay-API3 Plugin for SDRPlay: Source code and binary for Windows 10 (SDRSharp.SDRplay.dll)
<br/><br/>This plugin is taken from https://github.com/F6FLT/SDRSharp_v17xx-Plugin-for-SDRPlay and
adapted to allow the RSP1 and RSP1A SDRPlay to work with 2019 versions of SDR# (v 1.0.0.0.1716 -> 1732) and recent SDRplay API 3.11-3.15.

<b>WORKS WITH CURRENT SDRPLAY API 3.1x AND RSP1 and RSP1A SDR HARDWARE</b>

<b>You only want the turnkey solution for Windows</b>
- download and put the file SDRSharp.SDRplay.dll in the SDR# install folder
- with a text editor, open the file "FrontEnds.xml" in the SDR# install folder and add the line : 
<br/> &#60;add key="SDRplay" value="SDRSharp.SDRplay.SDRplayIO,SDRSharp.SDRplay"/&#62;
<br/> in the "&#60;frontendPlugins&#62;" section.
<br>The SDRPlay will appear in the list of devices that can be selected from SDR#


<b>You are interested in source code and development</b>
<br>To build, with Microsoft Visual Studio 2017:
- Open the .sln file in Visual Studio
- Add SDRSharp.Radio.dll and SDRSharp.Common.dll from your SDR# install folder as external references.

<br>
