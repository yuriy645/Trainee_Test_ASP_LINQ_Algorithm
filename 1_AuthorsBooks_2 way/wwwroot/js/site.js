// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
// Write your JavaScript code.
//Author/Index
    function openAuthorForm() {
        document.querySelector('.form-popupAuthors').style.display = 'block';
    }
    function sendAuthorForm() {
        let data = new FormData();
        data.append("newAuthor.FirstName.FirstName1", document.getElementById("firstName").value);
        data.append("newAuthor.AuthorSecondName", document.getElementById("secondName").value);
        data.append("newAuthor.Patronymic.Patronymic1", document.getElementById("thirdName").value);
        let xhr = new XMLHttpRequest();
        xhr.open("post", "author/AddAuthor");
        xhr.onload = () => {
            let response = JSON.parse(xhr.response);
            alert(
                response.message
            );
        }
        xhr.send(data); // отправка данных формы, вместе с остальными данными запроса
    }
    function closeAuthorForm() {
        document.getElementById("authorForm").style.display = "none";
    }	
	//Author/ViewAuthor
	function openForm(authorId, authorSecondName) {
        document.getElementById("authorSecondNameField").innerHTML = authorSecondName;
        document.getElementById("authorId").value = authorId;
        document.querySelector('.form-popup1').style.display = 'block';
    }    
    function sendBookForm() {
        let data = new FormData();
        data.append("authorId", document.getElementById("authorId").value);
        data.append("book.BookName", document.getElementById("bookName").value);
        data.append("book.Genre.GenreName", document.getElementById("genre").value);
        data.append("book.Pages", document.getElementById("pages").value);        
        let xhr = new XMLHttpRequest();
        xhr.open("post", "AddBook");
        xhr.onload = () => {
            let response = JSON.parse(xhr.response);
            alert(
                    response.message
                //response.bookName + "\n" +
                //response.genreName + "\n" +
                //response.pages
            );
        }
        xhr.send(data); // отправка данных формы, вместе с остальными данными запроса
    }
    function closeBookForm() {
        document.getElementById("bookForm").style.display = "none";
    }
    function saveData() {
        let data = new FormData();
        let xhr = new XMLHttpRequest();
        xhr.open("post", "Home/SaveData");
        xhr.send(data);
    }