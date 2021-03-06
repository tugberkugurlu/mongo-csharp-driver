﻿/* Copyright 2010-2014 MongoDB Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace MongoDB.Driver.GeoJsonObjectModel.Serializers
{
    /// <summary>
    /// Represents a serializer for a GeoJsonMultiLineStringCoordinates value.
    /// </summary>
    public class GeoJsonMultiLineStringCoordinatesSerializer<TCoordinates> : ClassSerializerBase<GeoJsonMultiLineStringCoordinates<TCoordinates>> where TCoordinates : GeoJsonCoordinates
    {
        // private fields
        private readonly IBsonSerializer<GeoJsonLineStringCoordinates<TCoordinates>> _lineStringCoordinatesSerializer = BsonSerializer.LookupSerializer<GeoJsonLineStringCoordinates<TCoordinates>>();

        // public methods
        /// <summary>
        /// Deserializes a value.
        /// </summary>
        /// <param name="context">The deserialization context.</param>
        /// <returns>The value.</returns>
        public override GeoJsonMultiLineStringCoordinates<TCoordinates> Deserialize(BsonDeserializationContext context)
        {
            var bsonReader = context.Reader;

            if (bsonReader.GetCurrentBsonType() == BsonType.Null)
            {
                bsonReader.ReadNull();
                return null;
            }
            else
            {
                var lineStrings = new List<GeoJsonLineStringCoordinates<TCoordinates>>();

                bsonReader.ReadStartArray();
                while (bsonReader.ReadBsonType() != BsonType.EndOfDocument)
                {
                    var lineString = _lineStringCoordinatesSerializer.Deserialize(context.CreateChild(_lineStringCoordinatesSerializer.ValueType));
                    lineStrings.Add(lineString);
                }
                bsonReader.ReadEndArray();

                return new GeoJsonMultiLineStringCoordinates<TCoordinates>(lineStrings);
            }
        }

        /// <summary>
        /// Serializes a value.
        /// </summary>
        /// <param name="context">The serialization context.</param>
        /// <param name="value">The value.</param>
        protected override void SerializeValue(BsonSerializationContext context, GeoJsonMultiLineStringCoordinates<TCoordinates> value)
        {
            var bsonWriter = context.Writer;

            bsonWriter.WriteStartArray();
            foreach (var lineString in value.LineStrings)
            {
                context.SerializeWithChildContext(_lineStringCoordinatesSerializer, lineString);
            }
            bsonWriter.WriteEndArray();
        }
    }
}
