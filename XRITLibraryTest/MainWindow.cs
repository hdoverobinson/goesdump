﻿using System;
using Gtk;
using System.IO;
using OpenSatelliteProject;
using OpenSatelliteProject.Tools;
using System.Drawing;
using System.Drawing.Imaging;
using OpenSatelliteProject.Log;
using OpenSatelliteProject.DCS;
using System.Collections.Generic;
using OpenSatelliteProject.PacketData.Enums;
using System.Threading;
using DotSpatial.Data;
using OpenSatelliteProject.PacketData;
using System.Text.RegularExpressions;
using System.Globalization;
using OpenSatelliteProject.Geo;

public partial class MainWindow: Gtk.Window {

    //DemuxManager dm;

    public MainWindow() : base(Gtk.WindowType.Toplevel) {
        Build();

        fileChooser.FileSet += (object sender, EventArgs e) => {
            Console.WriteLine(fileChooser.Filename);
            ProcessFile(fileChooser.Filename);
        };

        string filename = "./OR_ABI-L2-CMIPF-M3C07_G16_s20170861545382_e20170861556160_c20170861556217.lrit";
        XRITHeader header = FileParser.GetHeaderFromFile(filename);
        Console.WriteLine($"Parsing file {header.Filename}");
        Regex x = new Regex(@".*\((.*)\)", RegexOptions.IgnoreCase);
        var regMatch = x.Match(header.ImageNavigationHeader.ProjectionName);
        float satelliteLongitude = float.Parse(regMatch.Groups[1].Captures[0].Value, CultureInfo.InvariantCulture);
        var inh = header.ImageNavigationHeader;
        var gc = new GeoConverter(satelliteLongitude, inh.ColumnOffset, inh.LineOffset, inh.ColumnScalingFactor, inh.LineScalingFactor);

        var od = new OrganizerData();
        od.Segments.Add(0, filename);
        od.Columns = header.ImageStructureHeader.Columns;
        od.Lines = header.ImageStructureHeader.Lines;
        od.ColumnOffset = inh.ColumnOffset;
        od.PixelAspect = 1;
        var bmp = ImageTools.GenerateFullImage(od);

        var mapDrawer = new MapDrawer("/home/lucas/Works/OpenSatelliteProject/split/borders/ne_10m_admin_1_states_provinces.shp");
        mapDrawer.DrawMap(ref bmp, gc, Color.Aqua);

        ImageTools.DrawLatLonLines(ref bmp, gc, Color.Brown);

        bmp.Save(filename + ".jpg", ImageFormat.Jpeg);
        bmp.Dispose();

        /*
        Bitmap test0 = (Bitmap) Bitmap.FromFile("test0.jpg");
        Bitmap test1 = (Bitmap) Bitmap.FromFile("test1.jpg");
        Bitmap overlay = (Bitmap) Bitmap.FromFile("goes13-fulldisk.jpg");

        test0 = test0.ToFormat(PixelFormat.Format24bppRgb, true);

        overlay.Save("hue.jpg", ImageFormat.Jpeg);

        ImageTools.ApplyOverlay(ref test0, overlay);
        test0.Save("test0-ovl.jpg", ImageFormat.Jpeg);

        ImageTools.ApplyOverlay(ref test1, overlay);
        test1.Save("test1-ovl.jpg", ImageFormat.Jpeg);

        test0.Dispose();
        test1.Dispose();
        overlay.Dispose();
        */

        //string dcsFile = "/home/lucas/Works/OpenSatelliteProject/split/goesdump/XRITLibraryTest/bin/Debug/channels/DCS/pM-17085003239-A.dcs";
        //List<DCSHeader> d = DCSParser.parseDCS(dcsFile);
        ///*
        //string debugFrames = "/media/ELTN/tmp/demuxdump-1490627438.bin";
        //string debugFrames = "/media/ELTN/tmp/debug5/demuxdump-1492732814.bin";
        //string debugFrames = "/home/lucas/Works/OpenSatelliteProject/split/issues/trango/3/debug_frames.bin";
        /*
        string debugFrames = "/media/ELTN/tmp/debug3/raw_data.bin";
        var im0 = new ImageManager("channels/Images/Full Disk/");
        var im1 = new ImageManager("channels/Images/Northern Hemisphere/");
        var im2 = new ImageManager("channels/Images/Southern Hemisphere/");
        var im3 = new ImageManager("channels/Images/Area of Interest/");
        var im4 = new ImageManager("channels/Images/United States/");
        var im5 = new ImageManager("channels/Images/FM1/");

        ImageManager.GenerateVisible = true;
        ImageManager.GenerateInfrared = true;
        ImageManager.GenerateFalseColor = true;
        ImageManager.EraseFiles = true;
        im0.Start();
        im1.Start();
        im2.Start();
        im3.Start();
        im4.Start();
        im5.Start();
        */
        /*
        dm = new DemuxManager();
        FileHandler.SkipDCS = true;
        FileHandler.SkipEMWIN = true;
        //int startFrame = 956000;
        int startFrame = 0;
        FileStream file = File.OpenRead(debugFrames);
        byte[] data = new byte[892];
        long bytesRead = startFrame * 892;
        long bytesToRead = file.Length;
        int frameN = startFrame;
        file.Position = bytesRead;
        while (bytesRead < bytesToRead) {
            if (frameN % 1000 == 0) {
                //Console.WriteLine("Injecting Frame {0}", frameN);
            }
            bytesRead += file.Read(data, 0, 892);
            dm.parseBytes(data);
            frameN++;
        }

        Console.WriteLine("CRC Fails: {0}", dm.CRCFails);
        Console.WriteLine("Bugs: {0}", dm.Bugs);
        Console.WriteLine("Frame Loss: {0}", dm.FrameLoss);
        Console.WriteLine("Length Fails: {0}", dm.LengthFails);
        Console.WriteLine("Packets: {0}", dm.Packets);

        Console.WriteLine("Received Products: ");
        foreach (int pID in dm.productsReceived.Keys) {
            Console.WriteLine("\t{0}: {1}", ((NOAAProductID)pID).ToString(), dm.productsReceived[pID]);
        }
        //*/
        //im.Stop();
        //*/
        //ProcessFile("/home/lucas/Works/OpenSatelliteProject/split/goesdump/goesdump/bin/Debug/channels/Text/NWSTEXTdat043204159214.lrit");
        /*
        Organizer organizer = new Organizer("/home/lucas/Works/OpenSatelliteProject/split/goesdump/goesdump/bin/Debug/channels/Images/Full Disk");
        organizer.Update();

        var data = organizer.GroupData;

        foreach (var z in data) {
            var mData = z.Value;
            var bmp = ImageTools.GenerateFalseColor(mData);

            if (bmp != null) {
                bmp.Save(string.Format("{0}-{1}-{2}-{3}.jpg", mData.SatelliteName, mData.RegionName, "FLSCLR", z.Key), ImageFormat.Jpeg);
                bmp.Dispose();
                mData.IsProcessed = true;
            } else {
                if (mData.Visible.IsComplete && mData.Visible.MaxSegments != 0) {
                    bmp = ImageTools.GenerateFullImage(mData.Visible);
                    bmp.Save(string.Format("{0}-{1}-{2}-{3}.jpg", mData.SatelliteName, mData.RegionName, "VIS", z.Key), ImageFormat.Jpeg);
                    bmp.Dispose();
                }
                if (mData.Infrared.IsComplete && mData.Infrared.MaxSegments != 0) {
                    bmp = ImageTools.GenerateFullImage(mData.Infrared);
                    bmp.Save(string.Format("{0}-{1}-{2}-{3}.jpg", mData.SatelliteName, mData.RegionName, "IR", z.Key), ImageFormat.Jpeg);
                    bmp.Dispose();
                }
                if (mData.WaterVapour.IsComplete && mData.WaterVapour.MaxSegments != 0) {
                    bmp = ImageTools.GenerateFullImage(mData.WaterVapour);
                    bmp.Save(string.Format("{0}-{1}-{2}-{3}.jpg", mData.SatelliteName, mData.RegionName, "WV", z.Key), ImageFormat.Jpeg);
                    bmp.Dispose();
                }
                Console.WriteLine("Not all segments available!");
            }
        }


        //*/

        /*
        string visFile = "/home/lucas/Works/OpenSatelliteProject/split/samples/FD 26-02-17 2106 G13VI.jpg";
        string irFile = "/home/lucas/Works/OpenSatelliteProject/split/samples/FD 26-02-17 2106 G13IR.jpg";

        Bitmap vis = new Bitmap(visFile);
        ImageTools.ApplyCurve(Presets.VIS_FALSE_CURVE, ref vis);
        vis.Save("test.jpg", ImageFormat.Jpeg);
        //vis = vis.ToFormat(PixelFormat.Format32bppArgb, true);

        Bitmap ir = new Bitmap(irFile);
        ir = ir.ToFormat(PixelFormat.Format32bppArgb, true);
        ImageTools.ApplyLUT(Presets.THERMAL_FALSE_LUT, ref ir, 3);
        ir.Save("test2.jpg", ImageFormat.Jpeg);

        ir = ir.ToFormat(PixelFormat.Format32bppArgb);
        ImageTools.CombineHStoV(ref ir, vis);

        ir.Save("final.jpg", ImageFormat.Jpeg);
        //*/
    }

    private void ProcessFile(string filename) {
        //string outputFolder = System.IO.Path.GetDirectoryName(filename);
        //ImageHandler.Handler.HandleFile(filename, outputFolder);
        //TextHandler.Handler.HandleFile(filename, outputFolder);
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a) {
        Application.Quit();
        a.RetVal = true;
    }
}
