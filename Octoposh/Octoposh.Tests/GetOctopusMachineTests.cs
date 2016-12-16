﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using NUnit.Framework;
using Octoposh.Cmdlets;
using Octoposh.Model;

namespace Octoposh.Tests
{
    [TestFixture]
    public class GetOctopusMachineTests
    {
        private static readonly string CmdletName = "Get-OctopusMachine";

        [Test]
        public void GetMachineBySingleName()
        {
            var parameters = new List<CmdletParameter> {new CmdletParameter()
            {
                Name = "MachineName", SingleValue = "North"
            }};


            var powershell = new CmdletRunspace().CreatePowershellcmdlet(CmdletName, typeof(GetOctopusMachine), parameters);
            var results = powershell.Invoke<List<OutputOctopusMachine>>();

            Assert.AreEqual(results[0].Count, 1);
            Console.WriteLine("Items Found:");
            foreach (var item in results[0])
            {
                Console.WriteLine(item.Name);
            }
        }

        [Test]
        public void GetMachineByMultipleNames()
        {
            var parameters = new List<CmdletParameter> {new CmdletParameter()
            {
                Name = "MachineName", MultipleValue = new[] {"North","South"}
            }};

            var powershell = new CmdletRunspace().CreatePowershellcmdlet(CmdletName, typeof(GetOctopusMachine), parameters);
            var results = powershell.Invoke<List<OutputOctopusMachine>>();

            Assert.AreEqual(results[0].Count, 2);

            Console.WriteLine("Items Found:");
            foreach (var item in results[0])
            {
                Console.WriteLine(item.Name);
            }
        }

        [Test]
        public void DontGetMachineIfNameDoesntMatch()
        {
            var machinename = "TotallyANameThatYoullNeverPutToAMachine";

            var parameters = new List<CmdletParameter> {new CmdletParameter()
            {
                Name = "MachineName", SingleValue = machinename
            }};

            var powershell = new CmdletRunspace().CreatePowershellcmdlet(CmdletName, typeof(GetOctopusMachine), parameters);
            var results = powershell.Invoke<List<OutputOctopusMachine>>();

            Assert.AreEqual(results[0].Count, 0);
        }

        [Test]
        public void GetMachineByCommunicationStyle()
        {
            var communicationStyles = new string[] { "ListeningTentacle", "PollingTentacle", "SSHEndpoint", "CloudRegion", "OfflineDrop" };

            foreach (var style in communicationStyles)
            {
                var parameters = new List<CmdletParameter> {new CmdletParameter()
                {
                    Name = "CommunicationStyle", SingleValue = style
                }};

                var powershell = new CmdletRunspace().CreatePowershellcmdlet(CmdletName, typeof(GetOctopusMachine), parameters);
                var results = powershell.Invoke<List<OutputOctopusMachine>>();

                if (results != null)
                {
                    foreach (var result in results[0])
                    {
                        Assert.AreEqual(result.CommunicationStyle, style);
                    }
                }
                else
                {
                    Assert.Inconclusive("No targets of the type [{0}] were found",style);
                }
            }


        }

    }
}
