const fileInput = document.querySelector(".custom-file-input");
const fileButton = document.querySelector(".custom-file-upload");
const btnMsg = fileButton.textContent;
fileInput.addEventListener("change", (e) => {
    const fileName = document.querySelector(".custom-file-input").files[0].name;
    fileButton.lastElementChild.textContent = `${btnMsg} - ${fileName}`;
});

setTimeout(() => {
    document.querySelector("div.tox-tinymce").setAttribute("style", "height: 300px;");
}, 500);
