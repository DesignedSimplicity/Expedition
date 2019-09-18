using System;
using System.Text;
using System.Collections;
using System.Drawing.Imaging;
using System.Reflection;
using System.IO;

using TagLib;
using TagLib.Jpeg;

namespace Expedition.Win.NavTree.Framework
{
    //For "IPTC State" use: /app13/irb/8bimiptc/iptc/Province\/State
    //For "IPTC Country" use: /app13/irb/8bimiptc/iptc/Country\/Primary Location Name
    //BitmapMetadata meta = (BitmapMetadata)myDecoder.Frames[0].Metadata;
    //MyInfo = meta.GetQuery("/app13/ifb/8bimiptc/WHATEVER") ;
    //strCountryMeta=JpegMeta.GetQuery(@"/app13/irb/8bimiptc/iptc/Country\/Primary Location Name").ToString();


    /*
    Adobe Lightroom	
    Creator
    exif/Artist
    iptc IIM/By-line
    xmp/dc:creator
    (under Default)
    Caption
    exif/ImageDescription 
    iptc IIM/Caption-Abstract
 
    (under IPTC Content)
    Description
    exif/ImageDescription
    iptc IIM/Caption-Abstract
    xmp/dc:description
    Title
    iptc IIM/ObjectName
    xmp/dc:title
    Headline
    iptc IIM/Headline
    xmp/photoshop:Headline	
    Keyword Set
    iptc IIM/Keywords
 
    Sublocation
    iptc IIM/Sub-Location
    Iptc4xmpCore:Location
    Image City
    iptc IIM/City
    xmp/photoshop:City 
    State/Province
    iptc IIM/Province/State
    xmp/photoshop:State 
    Image Country 
    iptc IIM/Country
    xmp/photoshop:Country
    */
    public class DossierExifInfo
    {
        // http://metadatadeluxe.pbworks.com/w/page/47662311/Top%2010%20List%20of%20Embedded%20Metadata%20Properties

        public DateTime? DateTaken;
        public string Keywords;

        /*
        1 = Horizontal (normal) 
        2 = Mirror horizontal 
        3 = Rotate 180 
        4 = Mirror vertical 
        5 = Mirror horizontal and rotate 270 CW 
        6 = Rotate 90 CW 
        7 = Mirror horizontal and rotate 90 CW 
        8 = Rotate 270 CW
        */
        public int Orientation;
        public int Rating;

        /*
         * Software
         * Latitude
         * Longitude
         * Altitude
         * ExposureTime
         * FNumber
         * ISOSpeedRatings
         * FocalLength
         * FocalLengthIn35mmFilm
         * 
         * Make
         * Model
         * Creator
         */

    }

    public class DossierImageInfo : DossierExifInfo
    {
        public void Start(string path)
        {
            TagLib.File file = null;

            try
            {
                file = TagLib.File.Create(path);
            }
            catch (TagLib.UnsupportedFormatException)
            {
                Console.WriteLine("UNSUPPORTED FILE: " + path);
                Console.WriteLine(String.Empty);
                Console.WriteLine("---------------------------------------");
                Console.WriteLine(String.Empty);
                return;
            }

            var image = file as TagLib.Image.File;
            if (file == null)
            {
                Console.WriteLine("NOT AN IMAGE FILE: " + path);
                Console.WriteLine(String.Empty);
                Console.WriteLine("---------------------------------------");
                Console.WriteLine(String.Empty);
                return;
            }

            Console.WriteLine(String.Empty);
            Console.WriteLine(path);
            Console.WriteLine(String.Empty);

            Console.WriteLine("Tags in object  : " + image.TagTypes);
            Console.WriteLine(String.Empty);

            Console.WriteLine("Comment         : " + image.ImageTag.Comment);
            Console.Write("Keywords        : ");
            foreach (var keyword in image.ImageTag.Keywords)
            {
                Console.Write(keyword + " ");
            }
            Console.WriteLine();
            Console.WriteLine("Rating          : " + image.ImageTag.Rating);
            Console.WriteLine("DateTime        : " + image.ImageTag.DateTime);
            Console.WriteLine("Orientation     : " + image.ImageTag.Orientation);
            Console.WriteLine("Software        : " + image.ImageTag.Software);
            Console.WriteLine("ExposureTime    : " + image.ImageTag.ExposureTime);
            Console.WriteLine("FNumber         : " + image.ImageTag.FNumber);
            Console.WriteLine("ISOSpeedRatings : " + image.ImageTag.ISOSpeedRatings);
            Console.WriteLine("FocalLength     : " + image.ImageTag.FocalLength);
            Console.WriteLine("FocalLength35mm : " + image.ImageTag.FocalLengthIn35mmFilm);
            Console.WriteLine("Make            : " + image.ImageTag.Make);
            Console.WriteLine("Model           : " + image.ImageTag.Model);

            if (image.Properties != null)
            {
                Console.WriteLine("Width           : " + image.Properties.PhotoWidth);
                Console.WriteLine("Height          : " + image.Properties.PhotoHeight);
                Console.WriteLine("Type            : " + image.Properties.Description);
            }

            Console.WriteLine();
            Console.WriteLine("Writable?       : " + image.Writeable.ToString());
            Console.WriteLine("Corrupt?        : " + image.PossiblyCorrupt.ToString());

            if (image.PossiblyCorrupt)
            {
                foreach (string reason in image.CorruptionReasons)
                {
                    Console.WriteLine("    * " + reason);
                }
            }

            Console.WriteLine("---------------------------------------");
        }
    }
}