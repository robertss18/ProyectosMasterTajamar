import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { RestDataSource } from "../services/RestDataService";
import "./BookDetail.css";

export function BookDetail() {
    const { id } = useParams(); 
    const [book, setBook] = useState(null);
    const navigate = useNavigate();
    const dataSource = new RestDataSource("http://localhost:3500/books");

    useEffect(() => {
        dataSource.GetOne(id, (data) => setBook(data));
    }, [id, dataSource]);

    const deleteBook = () => {
        if (window.confirm(`¿Estas seguro de eliminar el libro "${book.title}"?`)) {
            dataSource.Delete(book, () => {
                alert("Libro eliminado correctamente.");
                navigate("/books"); 
            });
        }
    };

    if (!book) {
        return <div>Cargando...</div>;
    }

    return (
        <div className="d-flex justify-content-center align-items-center min-vh-100 bg-light">
            <div className="col-md-4 col-xl-3">
                <div className="card border-0 shadow-sm">
                    <div className="card-body p-4">
                        <div className="text-center mb-4">
                            <i className="fa fa-book fa-3x text-primary mb-3"></i>
                            <h3 className="card-title">{book.title}</h3>
                            <p className="card-subtitle text-muted">por {book.author}</p>
                        </div>

                        <div className="mb-4">
                            <p className="mb-2">
                                <strong>Anio de publicacion:</strong> {book.year}
                            </p>
                        </div>

                        <div className="d-grid gap-2">
                            <button className="btn btn-danger btn-sm d-flex align-items-center justify-content-center" onClick={deleteBook}>
                                <i className="fa fa-trash mx-1"></i> Eliminar libro
                            </button>
                            <button className="btn btn-outline-secondary btn-sm d-flex align-items-center justify-content-center" onClick={() => navigate(-1)}>
                                <i className="fa fa-arrow-left mx-1"></i> Volver atras
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}           
