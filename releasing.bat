:: LangLib
:: 
:: A program and library for application localization.
:: Copyright (C) 2015 VPKSoft, Petteri Kautonen
:: 
:: Contact: vpksoft@vpksoft.net
:: 
:: This file is part of LangLib.
:: 
:: LangLib is free software: you can redistribute it and/or modify
:: it under the terms of the GNU Lesser General Public License as published by
:: the Free Software Foundation, either version 3 of the License, or
:: (at your option) any later version.
:: 
:: LangLib is distributed in the hope that it will be useful,
:: but WITHOUT ANY WARRANTY; without even the implied warranty of
:: MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
:: GNU Lesser General Public License for more details.
:: 
:: You should have received a copy of the GNU Lesser General Public License
:: along with LangLib.  If not, see <http://www.gnu.org/licenses/>.


copy .\LangLibTestWinforms\bin\Release\LangLibTestWinforms.exe .\DBLocalization\bin\langlib_release\LangLibTestWinforms.exe
copy .\LangLibTestWPF\bin\Release\LangLibTestWPF.exe .\DBLocalization\bin\langlib_release\LangLibTestWPF.exe
copy .\SecureDatabaseSetting.exe .\DBLocalization\bin\langlib_release\SecureDatabaseSetting.exe 

copy .\DBLocalization\bin\Release\DBLocalization.exe .\DBLocalization\bin\langlib_release\DBLocalization.exe
copy .\DBLocalization\bin\Release\MySql.Data.dll .\DBLocalization\bin\langlib_release\MySql.Data.dll
copy .\DBLocalization\bin\Release\Npgsql.dll .\DBLocalization\bin\langlib_release\Npgsql.dll
copy .\DBLocalization\bin\Release\Npgsql.xml .\DBLocalization\bin\langlib_release\Npgsql.xml
copy .\DBLocalization\bin\Release\System.Data.SQLite.dll .\DBLocalization\bin\langlib_release\System.Data.SQLite.dll
copy .\DBLocalization\bin\Release\System.Data.SQLite.xml .\DBLocalization\bin\langlib_release\System.Data.SQLite.xml
copy .\DBLocalization\bin\Release\VPKSof.ConfLib.dll .\DBLocalization\bin\langlib_release\VPKSof.ConfLib.dll
copy .\DBLocalization\bin\Release\VPKSof.ConfLib.xml .\DBLocalization\bin\langlib_release\VPKSof.ConfLib.xml
copy .\DBLocalization\bin\Release\VPKSoft.LangLib.dll .\DBLocalization\bin\langlib_release\VPKSoft.LangLib.dll
copy .\DBLocalization\bin\Release\VPKSoft.LangLib.xml .\DBLocalization\bin\langlib_release\VPKSoft.LangLib.xml
copy .\DBLocalization\bin\Release\VPKSoft.Utils.dll .\DBLocalization\bin\langlib_release\VPKSoft.Utils.dll
copy .\DBLocalization\bin\Release\VPKSoft.Utils.xml .\DBLocalization\bin\langlib_release\VPKSoft.Utils.xml
copy .\DBLocalization\bin\Release\x86\SQLite.Interop.dll .\DBLocalization\bin\langlib_release\x86\SQLite.Interop.dll
copy .\DBLocalization\bin\Release\x64\SQLite.Interop.dll .\DBLocalization\bin\langlib_release\x64\SQLite.Interop.dll
copy .\DBLocalization\bin\Release\ .\DBLocalization\bin\langlib_release\





copy .\COPYING .\DBLocalization\bin\langlib_release
copy .\COPYING.LESSER .\DBLocalization\bin\langlib_release


pause