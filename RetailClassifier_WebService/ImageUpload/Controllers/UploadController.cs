using ImageUpload.Models;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.UI.WebControls;

namespace ImageUpload.Controllers
{
    [Route("api/classifierPrediction")]
    public class UploadController : ApiController
    {
        public static string SouthCentralUsEndpoint = ConfigurationManager.AppSettings["CustomVisionApiEndPoint"];
        public static string predictionKey = ConfigurationManager.AppSettings["CustomVisionPredictionKey"];
        public static Guid projectID;
        IList<Result> result;
        

        //[HttpPost]
        //public IHttpActionResult Post(Student student)
        //{
        //    db.Students.Add(student);l̥
        //    db.SaveChanges();
        //    return Ok();
        //}

        [HttpPost]
        public async Task<IList<Result>> PostImage(string imgpath)
        {

            //try
            //{
                var httpRequest = HttpContext.Current.Request;

            //    if (httpRequest.Files.Count > 0)
            //    {
            //        foreach (string file in httpRequest.Files)
            //        {
            //            var postedFile = httpRequest.Files[file];

            //            var fileName = postedFile.FileName.Split('\\').LastOrDefault().Split('/').LastOrDefault();

            //            var filePath = HttpContext.Current.Server.MapPath("~/Content/Upload/" + fileName);


            //            postedFile.SaveAs(filePath);

                        ImagePrediction prediction = Request(imgpath);

                        result = prediction.Predictions.Select(x => new Result { probability = (x.Probability *100), tagName = x.TagName }).ToList();

                        
                    
            
           
                return result;
                   
            

           
        }
        public ImagePrediction Request(string img)
        {
            FileStream fileStream = new FileStream(img, FileMode.Open, FileAccess.Read);
        
            CustomVisionPredictionClient endpoint = new CustomVisionPredictionClient()
            {
            ApiKey = predictionKey,
            Endpoint = SouthCentralUsEndpoint
             };
            projectID = new Guid(ConfigurationManager.AppSettings["CustomVisionProjectID"]);
            ImagePrediction result = endpoint.ClassifyImage(projectID, "Veg Classifier", fileStream);
            return result;

        }
}
}
