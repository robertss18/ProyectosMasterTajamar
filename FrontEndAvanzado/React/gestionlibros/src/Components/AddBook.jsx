import { useState } from "react";
import { RestDataSource } from "../services/RestDataService";
import { Link, useNavigate } from "react-router-dom";


export function AddBook() {
    const [title, setTitle] = useState("");
    const [author, setAuthor] = useState("");
    const [year, setYear] = useState("");
    const navigate = useNavigate();
    const dataSource = new RestDataSource("http://localhost:3500/books");

    const handleChange = (event) => {
        const { name, value } = event.target;

        if (name === "title") setTitle(value);
        else if (name === "author") setAuthor(value);
        else if (name === "year") setYear(value);
    };

    const handleSubmit = (event) => {
        event.preventDefault();
        const newBook = {
            title,
            author,
            year: parseInt(year, 10) 
        };

        dataSource.Store(newBook, () => {
            alert("Libro agregado correctamente");
            navigate("/books");
        });
    };

    return (
        <div className="container mt-4">
            <div className="card shadow-sm" style={{ maxWidth: "450px", margin: "auto" }}>
                <div className="card-header bg-primary text-white text-center">
                    <h5 className="mb-0">
                        <i className="fa fa-book"></i> Agregar Libro
                    </h5>
                </div>
                <div className="card-body">
                    <form className="form-horizontal" onSubmit={handleSubmit}>
                        <div className="form-group row">
                            <label className="col-sm-2 col-form-label" htmlFor="inputTitle">
                                <i className="fa fa-font"></i>
                            </label>
                            <div className="col-sm-10">
                                <input
                                    className="form-control"
                                    id="inputTitle"
                                    type="text"
                                    name="title"
                                    value={title}
                                    onChange={handleChange}
                                    placeholder="Titulo"
                                    required
                                />
                            </div>
                        </div>

                        <div className="form-group row mt-3">
                            <label className="col-sm-2 col-form-label" htmlFor="inputAuthor">
                                <i className="fa fa-user"></i>
                            </label>
                            <div className="col-sm-10">
                                <input
                                    className="form-control"
                                    id="inputAuthor"
                                    type="text"
                                    name="author"
                                    value={author}
                                    onChange={handleChange}
                                    placeholder="Autor"
                                    required
                                />
                            </div>
                        </div>

                        <div className="form-group row mt-3">
                            <label className="col-sm-2 col-form-label" htmlFor="inputYear">
                                <i className="fa fa-calendar"></i>
                            </label>
                            <div className="col-sm-10">
                                <input
                                    className="form-control"
                                    id="inputYear"
                                    type="number"
                                    name="year"
                                    value={year}
                                    onChange={handleChange}
                                    placeholder="Anio de Publicacion"
                                    required
                                />
                            </div>
                        </div>

                        <div className="form-group text-center mt-4">
                            <div className="row justify-content-center">
                                <button className="btn btn-info btn-sm col-4 mx-1" type="submit">
                                    <i className="fa fa-plus"></i> Confirmar
                                </button>
                                <Link className="btn btn-secondary btn-sm col-4 mx-1" to="../">
                                    <i className="fa fa-minus"></i> Cancelar
                                </Link>
                            </div>
                            
                        </div>
                    </form>
                </div>
            </div>
        </div>
    );

}