document.querySelector("#searchForm").addEventListener("submit", (e) => {
    const search = document.querySelector("#searchForm").firstElementChild.firstElementChild.value.trim();
    if (search === null || search === "" || search === undefined) {
        e.preventDefault();
        console.warn(`You can't search with no string input or whitespace`);
    }
    console.log(search);
});
