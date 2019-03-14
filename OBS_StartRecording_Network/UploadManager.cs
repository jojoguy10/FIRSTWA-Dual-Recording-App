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

    public struct YoutubeJob
    {
        public string filename;
        public string title;
        public string desc;
        public string tags;
        public string playlist;
        public YoutubeJob(string f, string t, string d, string g, string p)
        {
            filename = f;
            title = t;
            desc = d;
            tags = g;
            playlist = p;
        }
    }

    public partial class UploadManager : Form
    {
        private BufferBlock<YoutubeJob> todo;
        private bool alive;
        Task worker;

        public UploadManager(string pid)
        {
            InitializeComponent();

            alive = true;

            todo = new BufferBlock<YoutubeJob>();

            worker = Task.Run(() => Run());
        }

        public void addVideo(YoutubeJob task)
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

                YoutubeJob Job = todo.Receive();

                //retrieve client secret, renewing if needed, and create oauth credentials
                UserCredential credential;
                using (var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
                {
                    credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        // This OAuth 2.0 access scope allows an application to upload files to the
                        // authenticated user's YouTube channel, but doesn't allow other types of access.
                        new[] { YouTubeService.Scope.YoutubeUpload },
                        "user",
                        CancellationToken.None
                    );
                }

                //TODO: handle denied credentials gracefully

                //create service from credentials
                var youtubeService = new YouTubeService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = Assembly.GetExecutingAssembly().GetName().Name
                });

                var video = new Video();
                video.Snippet = new VideoSnippet();
                video.Snippet.Title = Job.title;
                video.Snippet.Description = Job.desc;

                video.Snippet.Tags = Job.tags.Split(',');
                //program_video.Snippet.CategoryId = "22"; // See https://developers.google.com/youtube/v3/docs/videoCategories/list
                video.Status = new VideoStatus();
                video.Status.PrivacyStatus = "public"; // or "private" or "public"
                var filePath = @"C:\Temp\" + Job.filename; // Replace with path to actual movie file.

                IUploadProgress progress;
                using (var fileStream = new FileStream(filePath, FileMode.Open))
                {
                    var videosInsertRequest = youtubeService.Videos.Insert(video, "snippet,status", fileStream, "video/*");

                    progress = videosInsertRequest.Upload();
                }

                if (progress.Status == UploadStatus.Completed)
                {
                    var newPlaylistItem = new PlaylistItem();
                    newPlaylistItem.Snippet = new PlaylistItemSnippet();
                    newPlaylistItem.Snippet.PlaylistId = Job.playlist; //REPLACE
                    newPlaylistItem.Snippet.ResourceId = new ResourceId();
                    newPlaylistItem.Snippet.ResourceId.Kind = "youtube#video";
                    newPlaylistItem.Snippet.ResourceId.VideoId = video.Id;
                    newPlaylistItem = youtubeService.PlaylistItems.Insert(newPlaylistItem, "snippet").Execute();

                    File.Delete(@"C:\Temp\" + Job.filename);
                }
                else
                {
                    MessageBox.Show("Failed to upload " + Job.filename);
                }
            }
        }

        ~UploadManager()
        {
            alive = false;
            worker.Wait();
        }
    }
}
