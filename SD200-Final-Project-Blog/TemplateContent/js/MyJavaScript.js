const makeCurrentPageHeaderNavLinkBold = (CurrentControllerMethodName) => {
    const indexNavLink = document.querySelector(".IndexPage");
    const blogNavLink = document.querySelector(".BlogPage");
    const postNavLink = document.querySelector(".PostPage");
    const currentPage = CurrentControllerMethodName;

    switch (currentPage) {
        case "Index":
            indexNavLink.classList.toggle("active");
            break;
        case "Blog":
            blogNavLink.classList.toggle("active");
            break;
        case "Post":
            postNavLink.classList.toggle("active");
            break;
    }
}
const arrayOfBlockQuotes = document.body.getElementsByTagName("blockquote");
for (let blockquote of arrayOfBlockQuotes) {
    console.log(blockquote);
    blockquote.classList.add("blockquote");
}