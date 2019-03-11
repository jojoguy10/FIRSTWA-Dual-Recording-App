using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace FIRSTWA_Recorder
{
    public partial class UploadManager : Form
    {
        private BufferBlock<YoutubeUpload> todo;
        private bool alive;
        Task worker;
        string playlistId;

        public UploadManager(string pid)
        {
            InitializeComponent();

            alive = true;

            todo = new BufferBlock<YoutubeUpload>();

            playlistId = pid;

            worker = Task.Run(() => Run());

            
        }

        public void addVideo(YoutubeUpload task)
        {
            todo.Post(task);
        }

        private async Task Run()
        {
            while (alive)
            {
                if (todo.Count == 0)
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("Spinning Wheels");
                    continue;
                }

                YoutubeUpload Job = todo.Receive();

                //post that shit
                UserCredential credential;
                using (var stream = new FileStream("C:\\secrets\\client_secrets.json", FileMode.Open, FileAccess.Read))
                {
                    credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        // This OAuth 2.0 access scope allows an application to upload files to the
                        // authenticated user's YouTube channel, but doesn't allow other types of access.
                        new[] { YouTubeService.Scope.YoutubeUpload },
                        "recorder-test@firstwa-recorder-beta.iam.gserviceaccount.com",
                        CancellationToken.None
                    );
                }

                var youtubeService = new YouTubeService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = Assembly.GetExecutingAssembly().GetName().Name
                });

                {
                    var program_video = new Video();
                    program_video.Snippet = new VideoSnippet();
                    program_video.Snippet.Title = Job.txtProgramTitle.Text;
                    program_video.Snippet.Description = Job.txtDescription.Text;

                    program_video.Snippet.Tags = Job.txtTags.Text.Split(',');
                    //program_video.Snippet.CategoryId = "22"; // See https://developers.google.com/youtube/v3/docs/videoCategories/list
                    program_video.Status = new VideoStatus();
                    program_video.Status.PrivacyStatus = "public"; // or "private" or "public"
                    var filePath = @"C:\Temp\" + Job.programFileName; // Replace with path to actual movie file.

                    using (var fileStream = new FileStream(filePath, FileMode.Open))
                    {
                        var videosInsertRequest = youtubeService.Videos.Insert(program_video, "snippet,status", fileStream, "video/*");
                        videosInsertRequest.ProgressChanged += videosInsertRequest_ProgressChanged;

                        videosInsertRequest.Upload();
                    }

                    var newPlaylistItem = new PlaylistItem();
                    newPlaylistItem.Snippet = new PlaylistItemSnippet();
                    newPlaylistItem.Snippet.PlaylistId = playlistId; //REPLACE
                    newPlaylistItem.Snippet.ResourceId = new ResourceId();
                    newPlaylistItem.Snippet.ResourceId.Kind = "youtube#video";
                    newPlaylistItem.Snippet.ResourceId.VideoId = program_video.Id;
                    newPlaylistItem = youtubeService.PlaylistItems.Insert(newPlaylistItem, "snippet").Execute();
                }

                {
                    var wide_video = new Video();
                    wide_video.Snippet = new VideoSnippet();
                    wide_video.Snippet.Title = Job.txtWideTitle.Text;
                    wide_video.Snippet.Description = Job.txtDescription.Text;

                    wide_video.Snippet.Tags = Job.txtTags.Text.Split(',');
                    //program_video.Snippet.CategoryId = "22"; // See https://developers.google.com/youtube/v3/docs/videoCategories/list
                    wide_video.Status = new VideoStatus();
                    wide_video.Status.PrivacyStatus = "public"; // or "private" or "public"
                    var filePath = @"C:\Temp\" + Job.programFileName; // Replace with path to actual movie file.

                    using (var fileStream = new FileStream(filePath, FileMode.Open))
                    {
                        var videosInsertRequest = youtubeService.Videos.Insert(wide_video, "snippet,status", fileStream, "video/*");
                        videosInsertRequest.ProgressChanged += videosInsertRequest_ProgressChanged;

                        videosInsertRequest.Upload();
                    }

                    var newPlaylistItem = new PlaylistItem();
                    newPlaylistItem.Snippet = new PlaylistItemSnippet();
                    newPlaylistItem.Snippet.PlaylistId = playlistId; //REPLACE
                    newPlaylistItem.Snippet.ResourceId = new ResourceId();
                    newPlaylistItem.Snippet.ResourceId.Kind = "youtube#video";
                    newPlaylistItem.Snippet.ResourceId.VideoId = wide_video.Id;
                    newPlaylistItem = youtubeService.PlaylistItems.Insert(newPlaylistItem, "snippet").Execute();
                }

                File.Delete(@"C:\Temp\" + Job.programFileName);
                File.Delete(@"C:\Temp\" + Job.wideFileName);
                Job.Close();
            }
        }

        void videosInsertRequest_ProgressChanged(Google.Apis.Upload.IUploadProgress progress)
        {
            switch (progress.Status)
            {
                case UploadStatus.Uploading:
                    Console.WriteLine("{0} bytes sent.", progress.BytesSent);
                    break;

                case UploadStatus.Failed:
                    Console.WriteLine("An error prevented the upload from completing.\n{0}", progress.Exception);
                    break;
            }
        }

        public void kill()
        {
            alive = false;
        }

        ~UploadManager()
        {
            alive = false;
            worker.Wait();
        }
    }
}
