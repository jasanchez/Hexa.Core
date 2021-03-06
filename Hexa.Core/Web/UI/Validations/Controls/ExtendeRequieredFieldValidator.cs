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
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hexa.Core.Web.UI.Controls
{
	public class ExtendedRequiredFieldValidator : RequiredFieldValidator
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings"), UrlProperty]
		public virtual string ImageUrl
		{
			get
			{
				string imageUrl = string.Empty;
				object o = ViewState["ImageUrl"];
				if (o != null)
					imageUrl = ViewState["ImageUrl"].ToString();

				return imageUrl;
			}
			set
			{
				ViewState["ImageUrl"] = value;
			}
		}

		protected override void OnPreRender(EventArgs e)
		{
            base.OnPreRender(e);

			if (!string.IsNullOrEmpty(ImageUrl))
			{
				Image img = new Image();
                img.ID = "i" + ID;
				img.ToolTip = ErrorMessage;
				img.ImageUrl = ImageUrl;
				Controls.Add(img);
			}
		}

	}
}