﻿using System;
using System.Collections.Generic;
using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.LanguageVariants
{
    /// <summary>
    /// Represents content item variant model.
    /// </summary>
    public sealed class ContentItemVariantModel
    {
        /// <summary>
        /// Gets or sets item of the variant.
        /// </summary>
        [JsonProperty("item")]
        public ObjectIdentifier Item { get; set; }

        /// <summary>
        /// Gets or sets elements of the variant.
        /// </summary>
        [JsonProperty("elements")]
        public IEnumerable<dynamic> Elements { get; set; }

        /// <summary>
        /// Gets or sets language of the variant.
        /// </summary>
        [JsonProperty("language")]
        public Reference Language { get; set; }

        /// <summary>
        /// Gets or sets last modified timestamp of the content item.
        /// </summary>
        [JsonProperty("last_modified")]
        public DateTime? LastModified { get; set; }

        /// <summary>
        /// Gets or sets workflow steps of the content item.
        /// </summary>
        //todo set?
        [JsonProperty("workflow_step")]
        public ObjectIdentifier WorkflowStep { get; set; }
    }
}