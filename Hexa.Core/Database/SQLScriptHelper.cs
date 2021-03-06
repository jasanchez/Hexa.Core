﻿#region License

//===================================================================================
//Copyright 2010 HexaSystems Corporation
//===================================================================================
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//http://www.apache.org/licenses/LICENSE-2.0
//===================================================================================
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.
//===================================================================================

#endregion

using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Hexa.Core.Database
{

	/// <summary>
	/// Helper class to execute scripts against a Sql Server database.
	/// </summary>
	public sealed class SqlScriptHelper
    {
        private SqlScriptHelper()
		{ 
		}

		/// <summary>
		/// Executes the specified script.
		/// </summary>
		/// <param name="connection">The connection.</param>
		/// <param name="script">The script.</param>
        public static void Execute(string connection, string script)
        {
            try
            {
				using (SqlConnection sqlConn = new SqlConnection(connection))
				{
					using (SqlCommand command = new SqlCommand())
					{
						command.Connection = sqlConn;
						sqlConn.Open();
						ExecuteCommands(command, GetCommandsFromScript(script));
						sqlConn.Close();
					}
				}
            }
            catch (SqlException e)
            {
				Console.WriteLine(e.Message);
            }
        }

		/// <summary>
		/// Gets the commands from the specified script.
		/// </summary>
		/// <param name="script">The script.</param>
		/// <returns></returns>
        private static string[] GetCommandsFromScript(string script)
        {
            return Regex.Split(script, "GO\r\n", RegexOptions.IgnoreCase);
        }

		/// <summary>
		/// Executes the commands.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <param name="sqlCommands">The SQL commands.</param>
        private static void ExecuteCommands(SqlCommand command, string[] sqlCommands)
        {
            foreach (string cmd in sqlCommands)
            {
                if (cmd.Length > 0)
                {
                    command.CommandText = cmd;
                    command.ExecuteNonQuery();
                }
            }
        }

    }
}
