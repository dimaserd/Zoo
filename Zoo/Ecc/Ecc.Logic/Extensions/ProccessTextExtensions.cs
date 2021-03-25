using Croco.Core.Contract.Models;
using Ecc.Contract.Models;
using Ecc.Contract.Models.Emails;
using Ecc.Logic.Abstractions;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ecc.Logic.Extensions
{
    public static class ProccessTextExtensions
    {
        static string ProccessReplacings(string text, List<KeyValuePair<string, string>> maskItems)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            foreach (var maskItem in maskItems)
            {
                text = text.Replace($"{{{maskItem.Key}}}", maskItem.Value);
            }

            return text;
        }

        public static SendEmailModel ToSendEmailModel(this SendEmailWithProccessTextModel model)
        {
            if (model.MaskItems == null)
            {
                model.MaskItems = new List<KeyValuePair<string, string>>();
            }

            return new SendEmailModel
            {
                Subject = ProccessReplacings(model.SubjectFormat, model.MaskItems),
                Body = ProccessReplacings(model.BodyFormat, model.MaskItems),
                AttachmentFileIds = model.AttachmentFileIds,
                Email = model.Email
            };
        }

        public static List<SendEmailModel> ToSendEmailModels(this MassSendMail model)
        {
            return model.EmailWithMasks.Select(x => new SendEmailModel
            {
                AttachmentFileIds = model.AttachmentFileIds,
                Body = ProccessReplacings(model.BodyFormat, x.MaskItems),
                Subject = ProccessReplacings(model.SubjectFormat, x.MaskItems),
                Email = x.Email
            }).ToList();
        }

        public static BaseApiResponse<SendEmailModel> ToSendEmailModel(this SendMailMessageViaHtmlTemplate model, IEccFilePathMapper filePathMapper)
        {   
            var filePath = filePathMapper.MapPath(model.TemplateFilePath);

            if (!File.Exists(filePath))
            {
                return new BaseApiResponse<SendEmailModel>(false, "Шаблон не найден по указаному пути");
            }

            var fileContents = File.ReadAllText(filePath);

            if(model.Replacings != null)
            {
                foreach (var replacing in model.Replacings)
                {
                    fileContents = fileContents.Replace(replacing.Key, replacing.Value);
                }
            }

            return new BaseApiResponse<SendEmailModel>(true, "Ok", new SendEmailModel
            {
                AttachmentFileIds = model.AttachmentFileIds,
                Body = fileContents,
                Email = model.Email,
                Subject = model.Subject
            });
        }
    }
}