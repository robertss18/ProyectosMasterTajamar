import Axios from "axios";

export class RestDataSource {
    constructor(base_url) {
        this.BASE_URL = base_url
    }

    async GetAll(callback) {
        this.SendRequest("get", this.BASE_URL, callback);
    }

    async GetOne(id, callback) {
        this.SendRequest("get", `${this.BASE_URL}/${id}`, callback);
    }

    async Store(data, callback) {
        console.log("data", data);
        this.SendRequest("post", this.BASE_URL, callback, data);
    }

    async Update(data, callback) {
        this.SendRequest("put", `${this.BASE_URL}/${data.id}`, (response) => {
            if (response) {
                console.log("Respuesta de la API:", response);
                callback(response);
            } else {
                console.error("Error al actualizar el libro");
                callback(null);
            }
        }, data);
    }


    async Delete(data, callback) {
        this.SendRequest("delete", `${this.BASE_URL}/${data.id}`, callback, data);
    }

    async SendRequest(method, url, callback, data = null) {
        try {
            const response = await Axios.request({ method, url, data });
            callback(response.data);
        } catch (error) {
            console.error("Error en la petición:", error);
        }
    }
}
