import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { BookList } from "./Components/BookList";
import { AddBook } from "./Components/AddBook";
import { EditBook } from "./Components/EditBook";
import { BookDetail } from "./Components/BookDetail";
import "./App.css";


function App() {
    return (
        <Router>
            <div className="container mt-4">
                <Routes>
                    <Route path="/" element={<BookList/>} /> 
                    <Route path="/books" element={<BookList/>} /> 
                    <Route path="/books/create" element={<AddBook/>} /> 
                    <Route path="/books/edit/:id" element={<EditBook/>} /> 
                    <Route path="/books/details/:id" element={<BookDetail/>} /> 
                </Routes>
            </div>
        </Router>
    )
}

export default App;
