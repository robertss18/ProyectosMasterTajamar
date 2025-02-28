import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { RestDataSource } from "../services/RestDataService";
import { Link } from "react-router-dom";

export function EditBook() {
    const { id } = useParams();
    const navigate = useNavigate();
    const [book, setBook] = useState({
        id: "",
        title: "",
        author: "",
        year: ""
    });

    const dataSource = new RestDataSource("http://localhost:3500/books");

    useEffect(() => {
        if (id) {
            dataSource.GetOne(id, (data) => {
                setBook({
                    id: data.id,
                    title: data.title,
                    author: data.author,
                    year: data.year
                });
            });
        }
    }, [id]);

    const handleChange = (event) => {
        const { name, value } = event.target;
        setBook({ ...book, [name]: value });
    };

    const handleSubmit = (event) => {
        event.preventDefault();
        const updatedBook = { ...book };

        dataSource.Update(updatedBook, (data) => {
            if (data) {
                alert("Libro actualizado correctamente");
                navigate("/books");
            } else {
                alert("Error al actualizar el libro");
            }
        });
    };

    return (
        <div className="container mt-4">
            <div className="card shadow-sm" style={{ maxWidth: "450px", margin: "auto" }}>
                <div className="card-header bg-warning text-white text-center">
                    <h5 className="mb-0">
                        <i className="fa fa-edit"></i> Editar Libro
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
                                    value={book.title}
                                    onChange={handleChange}
                                    placeholder="Título"
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
                                    value={book.author}
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
                                    value={book.year}
                                    onChange={handleChange}
                                    placeholder="Año de Publicación"
                                    required
                                />
                            </div>
                        </div>

                        <div className="form-group text-center mt-4">
                            <div className="row justify-content-center">
                                <button className="btn btn-success btn-sm col-4 mx-1" type="submit">
                                    <i className="fa fa-check"></i> Guardar
                                </button>
                                <Link className="btn btn-secondary btn-sm col-4 mx-1" to="/books">
                                    <i className="fa fa-times"></i> Cancelar
                                </Link>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    );
}
