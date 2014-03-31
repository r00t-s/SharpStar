﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Mono;
using SharpStar.Lib.DataTypes;
using SharpStar.Lib.Networking;

namespace SharpStar.Lib.Starbound
{
    public class Document
    {

        public string Name { get; set; }

        public int Version { get; set; }

        public Variant Data { get; set; }

        public Document()
        {
        }

        public Document(string name, int version, Variant data)
        {
            Name = name;
            Version = version;
            Data = data;
        }

        public static Document FromStream(IStarboundStream stream)
        {

            Document doc = new Document();
            doc.Name = stream.ReadString();

            stream.ReadUInt8();

            doc.Version = DataConverter.BigEndian.GetInt32(stream.ReadUInt8Array(4), 0);

            doc.Data = stream.ReadVariant();

            return doc;

        }

        public static List<Document> ListFromStream(byte[] data)
        {

            int discarded;

            var documents = new List<Document>();

            using (MemoryStream ms = new MemoryStream(data))
            {

                using (StarboundStream ss = new StarboundStream(ms))
                {

                    int len = (int)VLQ.ReadVLQ(ms, out discarded);


                    for (int i = 0; i < len; i++)
                        documents.Add(FromStream(ss));

                }

            }

            return documents;

        }

    }
}