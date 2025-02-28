import { Component } from "react";
import { RestDataSource } from "../services/RestDataService";
import "./BookList.css";
import { Link } from "react-router-dom";

export class BookList extends Component {
    constructor(props) {
        super(props);
        this.state = {
            books: [],
            filter: "",
            currentPage: 1,
            itemsPerPage: 4 
        };
        this.dataSource = new RestDataSource("http://localhost:3500/books");
    }

    componentDidMount() {
        this.dataSource.GetAll(data => this.setState({ books: data }));
    }

    handleFilterChange = (event) => {
        this.setState({ filter: event.target.value });
    };



    handlePageChange = (pageNumber) => {
        this.setState({ currentPage: pageNumber });
    };

    render() {
        const { books, filter, currentPage, itemsPerPage } = this.state;

        const filteredBooks = books.filter(book =>
            book.title.toLowerCase().includes(filter.toLowerCase())
        );

        const indexOfLastBook = currentPage * itemsPerPage;
        const indexOfFirstBook = indexOfLastBook - itemsPerPage;
        const currentBooks = filteredBooks.slice(indexOfFirstBook, indexOfLastBook);

        const totalPages = Math.ceil(filteredBooks.length / itemsPerPage);

        return (
            <div className="container mt-4">
                <div className="row">
                    <div className="col-12 mb-3 mb-lg-5">
                        <div className="overflow-hidden card table-nowrap table-card">
                            <div className="card-header d-flex justify-content-between align-items-center bg-info text-white">
                                <h5 className="mb-0">Lista de Libros</h5>
                                <input
                                    type="text"
                                    className="form-control w-25"
                                    placeholder="Buscar libro..."
                                    value={filter}
                                    onChange={this.handleFilterChange}
                                />
                                <Link className="btn btn-primary shadow btn-sm" to="/books/create">Agregar Libro</Link>
                            </div>
                            <div className="table-responsive">
                                <table className="table mb-0">
                                    <thead className="small text-uppercase bg-body text-muted ">
                                        <tr>
                                            <th>
                                                <i className="fa fa-book mx-1" aria-hidden="true" style={{ color: 'green' }}></i> Titulo
                                            </th>
                                            <th>
                                                <i className="fa fa-user mx-1" aria-hidden="true" style={{ color: 'purple' }}></i> Autor
                                            </th>
                                            <th>
                                                <i className="fa fa-calendar mx-1" aria-hidden="true" style={{ color: 'orange' }}></i> Anio de Publicacion
                                            </th>
                                            <th className="text-end">
                                                <i className="fa fa-cogs mx-1" aria-hidden="true" style={{ color: 'blue' }}></i> Accion
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {currentBooks.map(book => (
                                            <tr className="align-middle" key={book.id}>
                                                <td>{book.title}</td>
                                                <td>{book.author}</td>
                                                <td>{book.year}</td>
                                                <td className="text-end">
                                                    <div className="drodown">
                                                        <a data-bs-toggle="dropdown" href="#" className="btn p-1">
                                                            <i className="fa fa-bars" aria-hidden="true"></i>
                                                        </a>
                                                        <div className="dropdown-menu dropdown-menu-end">
                                                            <Link className="dropdown-item" to={`/books/details/${book.id}`} >
                                                                <i className="fa fa-eye" style={{ color: 'blue', marginRight: '8px' }}></i>
                                                                Ver Detalles
                                                            </Link>
                                                            <Link className="dropdown-item" to={`/books/edit/${book.id}`}>
                                                                <i className="fa fa-pencil" style={{ color: 'rosybrown', marginRight: '8px' }}></i>
                                                                Editar
                                                            </Link>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                        ))}
                                    </tbody>
                                </table>
                            </div>

                            <div className="d-flex justify-content-center mt-3">
                                <nav>
                                    <ul className="pagination">
                                        <li className="page-item">
                                            <button
                                                className="page-link"
                                                onClick={() => this.handlePageChange(currentPage - 1)}
                                                disabled={currentPage === 1}
                                            >
                                                Anterior
                                            </button>
                                        </li>
                                        {[...Array(totalPages)].map((_, index) => (
                                            <li key={index} className={`page-item ${currentPage === index + 1 ? 'active' : ''}`}>
                                                <button
                                                    className="page-link"
                                                    onClick={() => this.handlePageChange(index + 1)}
                                                >
                                                    {index + 1}
                                                </button>
                                            </li>
                                        ))}
                                        <li className="page-item">
                                            <button
                                                className="page-link"
                                                onClick={() => this.handlePageChange(currentPage + 1)}
                                                disabled={currentPage === totalPages}
                                            >
                                                Siguiente
                                            </button>
                                        </li>
                                    </ul>
                                </nav>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}
