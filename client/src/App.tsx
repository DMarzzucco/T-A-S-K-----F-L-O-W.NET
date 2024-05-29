import { BrowserRouter as Router, Routes, Route } from 'react-router-dom'
import './App.scss'
import { Footer, Header } from './components/Common';
import { Home, Message } from './pages';
function App() {
  return (
    <Router>
      <Routes>
        <Route path='/' element={
          <>
            <Header />
            <Home />
            <Footer />
          </>
        } />
        <Route path='/messages' element={
          <>
            <Header />
            <Message />
            <Footer />
          </>
        } />
      </Routes>
    </Router>
  )
}
export default App;