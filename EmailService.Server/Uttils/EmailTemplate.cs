
namespace EmailService.Server.Utils
{
    public static class EmailTemplate
    {

        /// <summary>
        /// This method generates an HTML email template for confirming user registration.
        /// It includes styling for basic formatting and a button to verify the email address.
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="verificationToken"></param>
        /// <returns></returns>
        public static string GenerateRegistrationConfirmationEmail(string domain, string verificationToken)
        {
            string emailBody = $@"
                <html>
                <head>
                    <style>
                        /* Your CSS styles here */
                        .container {{
                            max-width: 600px;
                            margin: 20px auto;
                            padding: 20px;
                            background-color: #ffffff;
                            border-radius: 8px;
                            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
                            text-align: center;
                        }}
                        h1 {{
                            color: #333333;
                            margin-bottom: 20px;
                        }}
                        p {{
                            color: #666666;
                            margin-bottom: 10px;
                        }}
                        .button {{
                            display: inline-block;
                            padding: 10px 20px;
                            background-color: #007bff;
                            color: #ffffff;
                            text-decoration: none;
                            border-radius: 4px;
                            color: #FFFFFF;
                        }}
                        .footer {{
                            margin-top: 30px;
                            padding-top: 20px;
                            border-top: 1px solid #dddddd;
                            text-align: center;
                            color: #999999;
                        }}
                    </style>
                </head>
                <body>
                    <div class=""container"">
                        <h1>Welcome to Our Application!</h1>
                        <p>Thank you for registering!</p>
                        <p>Please verify your email address by clicking the following link:</p>
                        <p><a class=""button"" href=""{domain}/verification/{verificationToken}"">Verify Email</a></p>
                    </div>
                    <div class=""footer"">
                        <p>Promact Team</p>
                    </div>
                </body>
                </html>";

            return emailBody;
        }

        /// <summary>
        /// This method generates an HTML email template for notifying users about a successful login.
        /// It includes styling for basic formatting and a message confirming the login.
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        public static string LoginEmailTemplate(string userEmail)
        {
            string template = $@"
            <!DOCTYPE html>
            <html lang=""en"">
            <head>
                <meta charset=""UTF-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <title>Login Success</title>
                <style>
                    body {{
                        font-family: 'Arial', sans-serif;
                        line-height: 1.6;
                        margin: 0;
                        padding: 0;
                        background-color: #f9f9f9;
                    }}
                    .container {{
                        max-width: 600px;
                        margin: 20px auto;
                        padding: 20px;
                        background-color: #ffffff;
                        border-radius: 8px;
                        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
                    }}
                    h1 {{
                        color: #333333;
                        margin-bottom: 20px;
                    }}
                    p {{
                        color: #666666;
                        margin-bottom: 10px;
                    }}
                    .button {{
                        display: inline-block;
                        padding: 10px 20px;
                        background-color: #007bff;
                        color: #ffffff;
                        text-decoration: none;
                        color: #FFFFFF;
                        border-radius: 4px;
                    }}
                    .footer {{
                        margin-top: 30px;
                        padding-top: 20px;
                        border-top: 1px solid #dddddd;
                        text-align: center;
                        color: #999999;
                    }}
                </style>
            </head>
            <body>
                <div class=""container"">
                    <h1>Login Success</h1>
                    <p>Hello {userEmail},</p>
                    <p>This is to inform you that your account has been successfully logged in.</p>
                    <p>If you did not perform this action, please contact our support team immediately.</p>
                    <p>Thank you!</p>
                </div>
                <div class=""footer"">
                    <p>Promact Team</p>
                </div>
            </body>
            </html>";

            return template;
        }

        /// <summary>
        /// This method generates an HTML email template for a forgot password request.
        /// It includes styling for basic formatting and a link to reset the password.
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="domain"></param>
        /// <param name="resetToken"></param>
        public static string ForgotPasswordTemplate(string userEmail, string domain, string resetToken)
        {
            string emailBody = $@"
                <html>
                <head>
                    <style>
                        body {{
                            font-family: 'Arial', sans-serif;
                            line-height: 1.6;
                            margin: 0;
                            padding: 0;
                            background-color: #f9f9f9;
                        }}
                        .container {{
                            max-width: 600px;
                            margin: 20px auto;
                            padding: 20px;
                            background-color: #ffffff;
                            border-radius: 8px;
                            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
                        }}
                        h1 {{
                            color: #333333;
                            margin-bottom: 20px;
                        }}
                        p {{
                            color: #666666;
                            margin-bottom: 10px;
                        }}
                        .button {{
                            display: inline-block;
                            padding: 10px 20px;
                            background-color: #007bff;
                            color: #ffffff;
                            text-decoration: none;
                            border-radius: 4px;
                        }}
     .footer {{
                            margin-top: 30px;
                            padding-top: 20px;
                            border-top: 1px solid #dddddd;
                            text-align: center;
                            color: #999999;
                        }}
                    </style>
                </head>
                <body>
                    <div class=""container"">
                        <h1>Password Reset Request</h1>
                        <p>Hello {userEmail},</p>
                        <p>You have requested to reset your password. Please click the following link to reset your password:</p>
                        <p><a href=""{domain}/resetpassword/{resetToken}"" class=""button"">Reset Password</a></p>
                        <p>This link will expire in 24 hours.</p>
                        <p>If you did not request this, please ignore this email.</p>
                        <p>Thank you!</p>
                    </div>
 <div class=""footer"">
                        <p>Promact Team</p>
                    </div>
                </body>
                </html>";

            return emailBody;

        }

        /// <summary>
        /// This method generates an HTML email template for confirming a successfully verified account.
        /// It includes styling for basic formatting and an icon indicating verification.
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        public static string GenerateVerifiedAccountEmail(string userEmail)
        {
            string emailBody = $@"
        <html>
        <head>
            <style>
                /* Your CSS styles here */
                body {{
                    font-family: 'Arial', sans-serif;
                    line-height: 1.6;
                    margin: 0;
                    padding: 0;
                    background-color: #f9f9f9;
                }}
                .container {{
                    max-width: 600px;
                    margin: 20px auto;
                    padding: 20px;
                    background-color: #ffffff;
                    border-radius: 8px;
                    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
                    text-align: center;
                }}
                h1 {{
                    color: #333333;
                    margin-bottom: 20px;
                }}
                p {{
                    color: #666666;
                    margin-bottom: 10px;
                }}
                svg {{
                    fill: #00cc00; 
                    width: 64px;
                    height: 64px;
                    margin-bottom: 20px;
                }}
                .footer {{
                    margin-top: 30px;
                    padding-top: 20px;
                    border-top: 1px solid #dddddd;
                    text-align: center;
                    color: #999999;
                }}
            </style>
        </head>
        <body>
            <div class=""container"">
                <h1>Account Verified</h1>
                <svg xmlns=""http://www.w3.org/2000/svg"" width=""64"" height=""64"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round"" class=""feather feather-check-circle"">
                    <path d=""M22 11.08V12a10 10 0 1 1-5.93-9.14"" />
                    <polyline points=""22 4 12 14.01 9 11.01"" />
                </svg>
                <p>Hello {userEmail},</p>
                <p>Congratulations! Your account has been successfully verified.</p>
                <p>You can now enjoy full access to our services.</p>
                <p>Thank you for choosing us!</p>
            </div>
            <div class=""footer"">
                <p>Promact Team</p>
            </div>
        </body>
        </html>";

            return emailBody;
        }

        /// <summary>
        /// This method generates an HTML email template for confirming a successful password reset.
        /// It includes styling for basic formatting and an icon indicating success.
        /// </summary>
        /// <param name="userEmail"></param>
        public static string GeneratePasswordResetConfirmationEmail(string userEmail)
        {
            string emailBody = $@"
                <html>
                <head>
                    <style>
                        /* Your CSS styles here */
                        .container {{
                            max-width: 600px;
                            margin: 20px auto;
                            padding: 20px;
                            background-color: #ffffff;
                            border-radius: 8px;
                            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
                            text-align: center;
                        }}
                        h1 {{
                            color: #333333;
                            margin-bottom: 20px;
                        }}
                        p {{
                            color: #666666;
                            margin-bottom: 10px;
                        }}
                        .icon {{
                            fill: #00cc00; /* Green color for the checkmark icon */
                            width: 64px;
                            height: 64px;
                            margin-bottom: 20px;
                        }}
                        .footer {{
                            margin-top: 30px;
                            padding-top: 20px;
                            border-top: 1px solid #dddddd;
                            text-align: center;
                            color: #999999;
                        }}
                    </style>
                </head>
                <body>
                    <div class=""container"">
                        <svg class=""icon"" xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 24 24""><path d=""M0 0h24v24H0z"" fill=""none""/><path d=""M9 16.2L4.8 12l-1.4 1.4L9 19 21 7l-1.4-1.4L9 16.2z""/></svg>
                        <h1>Password Reset Successful</h1>
                        <p>Hello {userEmail},</p>
                        <p>Your password has been successfully reset.</p>
                        <p>If you did not perform this action, please contact our support team immediately.</p>
                        <p>Thank you!</p>
                    </div>
                    <div class=""footer"">
                        <p>Promact Team</p>
                    </div>
                </body>
                </html>";

            return emailBody;
        }

    }
}
