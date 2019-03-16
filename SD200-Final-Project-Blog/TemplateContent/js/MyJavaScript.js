const makeCurrentPageHeaderNavLinkBold = (CurrentControllerMethodName) => {
    const indexNavLink = document.querySelector(".IndexPage");
    const blogNavLink = document.querySelector(".BlogPage");
    const createPostNavLink = document.querySelector(".CreatePostPage");
    const searchPostsNavLink = document.querySelector(".SearchPage");
    const currentPage = CurrentControllerMethodName;

    switch (currentPage) {
        case "Index":
            indexNavLink.classList.toggle("active");
            break;
        case "Blog":
            blogNavLink.classList.toggle("active");
            break;
        case "CreatePost":
            createPostNavLink.classList.toggle("active");
            break;
        case "SearchPosts":
            searchPostsNavLink.classList.toggle("active");
            break;
    }
}
const arrayOfBlockQuotes = document.body.getElementsByTagName("blockquote");
for (let blockquote of arrayOfBlockQuotes) {
    console.log(blockquote);
    blockquote.classList.add("blockquote");
}