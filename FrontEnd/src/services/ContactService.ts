import axios from "axios";

const API = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
});

export default {
  listContacts: async () => {
    const response = await API.get("/contacts");
    return response.data;
  },
  getContactById: async (id: string) => {
    const response = await API.get(`/contacts/${id}`);
    return response.data;
  },
  createContact: async (contactData: {
    name: string;
    email: string;
    phone: string;
  }) => {
    console.log("Creating contact with data:", contactData);
    const response = await API.post("/contacts", contactData);
    return response.data;
  },
  updateContact: async (
    id: string,
    contactData: { name?: string; email?: string; phone?: string }
  ) => {
    const response = await API.put(`/contacts/${id}`, contactData);
    return response.data;
  },
  deleteContact: async (id: string) => {
    const response = await API.delete(`/contacts/${id}`);
    return response.data;
  },
}