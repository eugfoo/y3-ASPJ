<%@ Page Title="" Language="C#" MasterPageFile="~/SiteBootstrap.Master" AutoEventWireup="true" CodeBehind="forumProjectClean.aspx.cs" Inherits="FinalProj.forumProjectClean" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <style type="text/css">
        .author-col {
            width: 12em;
        }

        .post-col {
            min-width: 20em;
        }
    </style>
    <div class="container my-3">
        <nav class="breadcrumb">
            <a href="forumPage1.aspx" class="breadcrumb-item">Board index</a>
            <a href="forumCatOverview.aspx" class="breadcrumb-item">Forum Category</a>
            <span class="breadcrumb-item active"> Project Beach Cleaning</span>
        </nav>
        <div class="row">
            <div class="col-12">
                <div class="container">
                    <div class="row text-white bg-info mb-0 p-4 rounded-top">
                        <div class="col-md-9">
                            <h2 class="h4">Project Beach Cleaning</h2>
                        </div>

                    </div>
                </div>

                <table class="table table-striped table-bordered table-responsive-lg">
                    <thead class="thead-light">
                        <tr>
                            <th scope="col"></th>
                            <th scope="col"></th>
                        </tr>
                    </thead>

                    <tbody>
                        <tr>
                            <td class="author-col">
                                <div><a href="#0"><strong>Kovi Tan</strong></a></div>
                            </td>
                            <td class="post-col d-lg-flex justify-content-lg-between">
                                <div><span class="font-weight-bold">Post subject:</span> Project Beach Cleaning</div>
                                <div><span class="font-weight-bold">Posted: </span>02 Apr 2019, 13:33</div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <img src="Img/organiser.jpeg" class="img-fluid" />

                                <div><span class="font-weight-bold">Joined: </span>02 Apr 2019, 23:59</div>
                                <div><span class="font-weight-bold">Posts:</span>123</div>
                            </td>
                            <td>
                                <img src="Img/eventphoto.jpeg" class="img-fluid rounded" />
                                  
                                This is about a project that is currently held in Singapore
                <br />
                                We look to making this thing world wide
                <br />
                                <p>
                                    Things to bring:
                <br />
                                    - water bottle (important)
                <br />
                                    - small bag
                <br />
                                    - yourself
                <br />
                                </p>
                                <p>
                                    In this exercise, the idea is to write a paragraph that would be a random passage from a story. An effective paragraph is one that has unity (it isn’t a hodgepodge of things), focus (everything in the paragraph stacks up to the whatever-it-is the paragraph is about), and coherence (the content follows smoothly). For this exercise, the paragraph should be quick to read--say, not be more than 100 words long.
                                </p>
                                <p>
                                    Lines of weeds criss-crossed the cracked parking lot of the Seashell Motor Courts. The flaking paint on the buildings had chalked to a pastel pink on walls covered with graffiti. Many of the windows had been smashed out. Where the sign had been, atop rusting steel posts, only the metal outline of a seashell remained.
                                </p>
                                <p>
                                    Lines of weeds criss-crossed the cracked parking lot of the Seashell Motor Courts. The flaking paint on the buildings had chalked to a pastel pink on walls covered with graffiti. Many of the windows had been smashed out. Where the sign had been, atop rusting steel posts, only the metal outline of a seashell remained.
                                </p>
                            </td>
                        </tr>
                    </tbody>
                </table>

                <table class="table table-striped table-bordered table-responsive-lg">
                    <tbody>
                        <tr>
                            <td class="author-col">
                                <div><a href="#0"><strong>Foogene</strong></a></div>
                            </td>
                            <td class="post-col d-lg-flex justify-content-lg-between">
                                <div><span class="font-weight-bold">Post subject:</span> Project Beach Cleaning</div>
                                <div><span class="font-weight-bold">Posted: </span>02 Apr 2019, 13:33</div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <img src="https://www.famousbirthdays.com/faces/tan-jianhao-image.jpg" class="img-fluid" />

                                <div><span class="font-weight-bold">Joined: </span>02 Apr 2019, 23:59</div>
                                <div><span class="font-weight-bold">Posts: </span>123</div>
                            </td>
                            <td>
                                <div>
                                    <p>
                                        Lorem Ipsum is simply dummy text of the printing and typesetting industry. 
                                    Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, 
                                    when an unknown printer took a galley of type and scrambled it to make a type
                                    specimen book. It has survived not only five centuries, but also the leap 
                                    into electronic typesetting, remaining essentially unchanged. It was popularised
                                    in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages
                                    , and more recently with desktop publishing software like Aldus PageMaker 
                                    including versions of Lorem Ipsum.
                                    </p>
                                </div>
                                <div class="container">
                                    <div class="row">
                                        <div class="col-sm-9" style="height: 200px;"></div>
                                        <div class="col-sm-3">
                                            <div style="margin-top: 200px;" class="float-sm-right float-lg-right float-lg-right">
                                                <a href="#" class="btn btn-primary"><span><strong>Edit</strong></span></a>
                                                <a href="#" class="btn btn-danger"><span><strong>Delete</strong></span></a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>



        <div class="mb-3 clearfix">
            <nav aria-label="Navigate post pages" class="float-lg-right">
                <ul class="pagination pagination-sm mb-lg-0">
                    <li class="page-item active"><a href="#0" class="page-link">1 <span class="sr-only">(current)</span></a></li>
                    <li class="page-item"><a href="#0" class="page-link">2</a></li>
                    <li class="page-item"><a href="#0" class="page-link">3</a></li>
                    <li class="page-item"><a href="#0" class="page-link">4</a></li>
                    <li class="page-item"><a href="#0" class="page-link">5</a></li>
                    <li class="page-item"><a href="#0" class="page-link">&hellip; 31</a></li>
                    <li class="page-item"><a href="#0" class="page-link">Next</a></li>
                </ul>
            </nav>
        </div>
        <form class="mb-3">
            <div class="form-group">
                <label for="comment">Reply to this post:</label>
                <textarea class="form-control" id="comment" rows="10" placeholder="Write your comment here." required></textarea>
            </div>
            <button type="submit" class="btn btn-primary">Reply</button>
            <button type="reset" class="btn btn-danger">Clear</button>
        </form>
    </div>


</asp:Content>
