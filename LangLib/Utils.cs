#region License
/*
LangLib

A program and library for application localization.
Copyright (C) 2019 VPKSoft, Petteri Kautonen

Contact: vpksoft@vpksoft.net

This file is part of LangLib.

LangLib is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

LangLib is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with LangLib.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace VPKSoft.LangLib
{
    /// <summary>
    /// Some utilities used in the LangLib
    /// </summary>
    public class Utils
    {
        /// <summary>
        /// Checks if a program was given a command line parameter
        /// <para/>"--dbLang" to notify that the program should localize
        /// <para/>it self.
        /// </summary>
        /// <returns>A CultureInfo if the program was given the command 
        /// <para/>line parameter "--dbLang", otherwise null.</returns>
        public static CultureInfo ShouldLocalize()
        {
            foreach (string arg in System.Environment.GetCommandLineArgs())
            {
                if (arg.ToUpper().StartsWith("--dbLang".ToUpper()))
                {
                    try
                    {
                        string cultureName = arg.Split('=')[1];
                        return CultureInfo.GetCultureInfo(cultureName);
                    }
                    catch
                    {
                        return CultureInfo.GetCultureInfo(1033);
                    }
                }
            }
            return null;
        }
    }
}
