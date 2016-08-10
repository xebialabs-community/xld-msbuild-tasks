using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using XebiaLabs.Deployit.Client.Manifest;
using XebiaLabs.Deployit.Client.Package;

namespace XebiaLabs.Deployit.MSbuildTasks
{
    public class CreatePackage : Task
    {
   
        [Required]
        public string ManifestPath { get; set; }

        [Required]
        public string ApplicationVersion { get; set; }

        [Required]
        public string PackageDataRootDirectory { get; set; }

        [Required]
        public string PackagePath { get; set; }

        [Output]
        public string ApplicationName { get; set; }

        public override bool Execute()
        {


            if (!File.Exists(ManifestPath))
            {
                Log.LogError("Cannont find manifest '{0}", ManifestPath);
                return false;
            }
            
            if (string.IsNullOrWhiteSpace(ApplicationVersion))
            {
                Log.LogError("Version not provided");
                return false;
            }

            if (!Directory.Exists(PackageDataRootDirectory))
            {
                Log.LogError("Package root directory'{0}' does not exist", PackageDataRootDirectory);
                return false;
            }
           
            DeployitManifest manifest;
            try
            {
                var reader = new StreamReader(ManifestPath, DeployitManifest.Encoding);
                using (reader)
                {
                    manifest = DeployitManifest.Load(reader);
                }
            }
            catch (Exception ex)
            {
                Log.LogErrorFromException(ex);
                return false;
            }
            manifest.Version = ApplicationVersion;

            ApplicationName = manifest.ApplicationName;
          
            PackageBuilder.Build(manifest, PackageDataRootDirectory, PackagePath);
            return true;
        }          
    }
}