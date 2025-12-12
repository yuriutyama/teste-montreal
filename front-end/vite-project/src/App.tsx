import { BrowserRouter, Navigate, Route, Routes } from "react-router-dom";
import "./App.css";
import Layout from "./components/Layout";
import AutorFormPage from "./pages/autores/AutorFormPage";
import AutoresPage from "./pages/autores/AutoresPage";
import GeneroFormPage from "./pages/generos/GeneroFormPage";
import GenerosPage from "./pages/generos/GenerosPage";
import LivroFormPage from "./pages/livros/LivroFormPage";
import LivrosPage from "./pages/livros/LivrosPage";

function App() {
  return (
    <BrowserRouter>
      <Layout>
        <Routes>
          <Route path="/" element={<LivrosPage />} />
          <Route path="/livros" element={<LivrosPage />} />
          <Route path="/livros/novo" element={<LivroFormPage />} />
          <Route path="/livros/:id/editar" element={<LivroFormPage />} />

          <Route path="/autores" element={<AutoresPage />} />
          <Route path="/autores/novo" element={<AutorFormPage />} />
          <Route path="/autores/:id/editar" element={<AutorFormPage />} />

          <Route path="/generos" element={<GenerosPage />} />
          <Route path="/generos/novo" element={<GeneroFormPage />} />
          <Route path="/generos/:id/editar" element={<GeneroFormPage />} />

          <Route path="*" element={<Navigate to="/" replace />} />
        </Routes>
      </Layout>
    </BrowserRouter>
  );
}

export default App;
