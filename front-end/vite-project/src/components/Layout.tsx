import { NavLink } from "react-router-dom";
import "../App.css";

type LayoutProps = {
  children: React.ReactNode;
};

export default function Layout({ children }: LayoutProps) {
  return (
    <div className="app-shell">
      <header className="app-header">
        <h1>Biblioteca</h1>
        <nav>
          <NavLink to="/livros" className="nav-link">
            Livros
          </NavLink>
          <NavLink to="/autores" className="nav-link">
            Autores
          </NavLink>
          <NavLink to="/generos" className="nav-link">
            Gêneros
          </NavLink>
        </nav>
      </header>
      <main className="app-main">{children}</main>
    </div>
  );
}

