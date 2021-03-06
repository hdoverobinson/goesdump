<?xml version="1.0"?>
<package >
  <metadata>
    <id>OpenSatelliteProject.XRIT</id>
    <version>|{|VERSION|}|</version>
    <title>XRIT Library</title>
    <authors>Lucas Teske</authors>
    <owners>Lucas Teske</owners>
    <licenseUrl>https://github.com/opensatelliteproject/goesdump/blob/master/XRIT/LICENSE</licenseUrl>
    <projectUrl>https://github.com/opensatelliteproject/goesdump/</projectUrl>
    <iconUrl>https://github.com/opensatelliteproject/goesdump/raw/master/goesdump/icon.png</iconUrl>
    <requireLicenseAcceptance>true</requireLicenseAcceptance>
    <description>xRIT File / Packet Processing Library</description>
    <releaseNotes>
	Added LogLevel Enable Control and reduced some log messages to contain only filename and not the full path
	Added new False Color LUT system
	Added better error handling when loading ShapeFiles / MapDrawer Verified
	Added Debug Message Prefix and some more Debug Messages Regarding ShapeFiles
	Added Revsion to LibInfo.Version
	Changed False Color LUT to be less yellow-ish and added Thermal / Radiance Offset Tunning
    </releaseNotes>
    <copyright>Copyright 2017</copyright>
    <tags>GOES XRIT NOAA Satellite OpenSatelliteProject OSP SDR Ingestor</tags>
    <dependencies>
      <dependency id="DotSpatial.Data" version="1.9"/>
      <dependency id="DotSpatial.Mono" version="1.9" />
      <dependency id="DotSpatial.Projections" version="1.9"/>
      <dependency id="DotSpatial.Serialization" version="1.9"/>
      <dependency id="DotSpatial.Topology" version="1.9"/>
      <dependency id="SharpZipLib" version="0.86.0"/>
      <dependency id="sqlite-net" version="1.0.8"/>
    </dependencies> 
  </metadata>
</package>
