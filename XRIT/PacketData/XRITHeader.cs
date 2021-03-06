﻿using System;
using System.Collections.Generic;
using OpenSatelliteProject.PacketData.Enums;

namespace OpenSatelliteProject.PacketData {
    public class XRITHeader {

        #region Stored Headers

        public AncillaryHeader AncillaryHeader { get; set; }

        public AnnotationHeader AnnotationHeader { get; set; }

        public DCSFilenameHeader DCSFilenameHeader { get; set; }

        public HeaderStructuredHeader HeaderStructuredHeader { get; set; }

        public ImageDataFunctionHeader ImageDataFunctionHeader { get; set; }

        public ImageNavigationHeader ImageNavigationHeader { get; set; }

        public ImageStructureHeader ImageStructureHeader { get; set; }

        public NOAASpecificHeader NOAASpecificHeader { get; set; }

        public PrimaryHeader PrimaryHeader { get; set; }

        public RiceCompressionHeader RiceCompressionHeader { get; set; }

        public SegmentIdentificationHeader SegmentIdentificationHeader { get; set; }

        public TimestampHeader TimestampHeader { get; set; }

        public List<XRITBaseHeader> UnknownHeaders { get; set; }

        /// <summary>
        /// Returns true if the the this is an instance of FullDisk
        /// PS: It may return false for Himawari / Goes 16 fulldisks.
        /// </summary>
        /// <value><c>true</c> if this instance is full disk; otherwise, <c>false</c>.</value>
        public bool IsFullDisk { get { return  PrimaryHeader.FileType == FileTypeCode.IMAGE &&
            (
                Product.ID == (int)NOAAProductID.GOES13_ABI ||
                Product.ID == (int)NOAAProductID.GOES15_ABI ||
                Product.ID == (int)NOAAProductID.GOES16_ABI ||
                Product.ID == (int)NOAAProductID.HIMAWARI8_ABI
            ) &&
            (
                SubProduct.ID == (int)ScannerSubProduct.INFRARED_FULLDISK ||
                SubProduct.ID == (int)ScannerSubProduct.VISIBLE_FULLDISK ||
                SubProduct.ID == (int)ScannerSubProduct.WATERVAPOUR_FULLDISK 
            );
            } 
        }

        #endregion

        #region Helper Getters

        public string Filename {
            get {
                if (DCSFilenameHeader != null) {
                    return DCSFilenameHeader.Filename;
                } else if (AnnotationHeader != null) {
                    return AnnotationHeader.Filename;
                } else {
                    return null;
                }
            }
        }

        public CompressionType Compression {
            get {
                if (NOAASpecificHeader != null) {
                    return NOAASpecificHeader.Compression;
                } else if (ImageStructureHeader != null) {
                    return ImageStructureHeader.Compression;
                } else {
                    return CompressionType.NO_COMPRESSION;
                }
            }
        }

        public bool IsCompressed {
            get {
                return Compression != CompressionType.NO_COMPRESSION;
            }
        }

        public NOAAProduct Product {
            get { 
                if (NOAASpecificHeader != null) {
                    return NOAASpecificHeader.Product;
                } else {
                    return new NOAAProduct(-1);
                }
            }
        }

        public NOAASubproduct SubProduct {
            get {
                if (NOAASpecificHeader != null) {
                    return NOAASpecificHeader.SubProduct;
                } else {
                    return new NOAASubproduct(-1);
                }
            }
        }

        #endregion

        #region Constructors

        public XRITHeader() {
            UnknownHeaders = new List<XRITBaseHeader>();
        }

        public XRITHeader(List<XRITBaseHeader> headers) {
            UnknownHeaders = new List<XRITBaseHeader>();

            foreach (XRITBaseHeader header in headers) {
                SetHeader(header);
            }
        }

        #endregion

        #region Methods

        public void SetHeader(XRITBaseHeader header) {
            switch (header.Type) {
                case HeaderType.AncillaryTextRecord:
                    AncillaryHeader = (AncillaryHeader)header;
                    break;
                case HeaderType.AnnotationRecord:
                    AnnotationHeader = (AnnotationHeader)header;
                    break;
                case HeaderType.DCSFileNameRecord:
                    DCSFilenameHeader = (DCSFilenameHeader)header;
                    break;
                case HeaderType.HeaderStructuredRecord:
                    HeaderStructuredHeader = (HeaderStructuredHeader)header;
                    break;
                case HeaderType.ImageDataFunctionRecord:
                    ImageDataFunctionHeader = (ImageDataFunctionHeader)header;
                    break;
                case HeaderType.ImageNavigationRecord:
                    ImageNavigationHeader = (ImageNavigationHeader)header;
                    break;
                case HeaderType.ImageStructureRecord:
                    ImageStructureHeader = (ImageStructureHeader)header;
                    break;
                case HeaderType.NOAASpecificHeader:
                    NOAASpecificHeader = (NOAASpecificHeader)header;
                    break;
                case HeaderType.PrimaryHeader:
                    PrimaryHeader = (PrimaryHeader)header;
                    break;
                case HeaderType.RiceCompressionRecord:
                    RiceCompressionHeader = (RiceCompressionHeader)header;
                    break;
                case HeaderType.SegmentIdentificationRecord:
                    SegmentIdentificationHeader = (SegmentIdentificationHeader)header;
                    break;
                case HeaderType.TimestampRecord:
                    TimestampHeader = (TimestampHeader)header;
                    break;
                default:
                    UnknownHeaders.Add(header);
                    break;
            }
        }

        public string ToNameString() {
            if (
                Product.ID == (int)NOAAProductID.GOES13_ABI || 
                Product.ID == (int)NOAAProductID.GOES15_ABI || 
                Product.ID == (int)NOAAProductID.GOES16_ABI ||
                Product.ID == (int)NOAAProductID.ABI_RELAY  ||
                Product.ID == (int)NOAAProductID.HIMAWARI8_ABI
            ) {
                string baseName = $"{Product.Name} - {SubProduct.Name}";

                if (SegmentIdentificationHeader != null) {
                    baseName = $"{baseName} (ID: {SegmentIdentificationHeader.ImageID} Seg: {SegmentIdentificationHeader.Sequence}/{SegmentIdentificationHeader.MaxSegments})";
                }

                return baseName;
            }


            // Fallback
            return (SubProduct.Name != "Unknown") ? $"{Product.Name} - {SubProduct.Name}" : Product.Name;
        }

        #endregion
    }
}

