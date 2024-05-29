import { useState } from 'react'
import '../App.scss'
import { Link } from 'react-router-dom';

const imagePath: string[] = [
    'img/eg1.jpg',
    'img/eg2.jpg'
]

function Home() {
    const [currentIndex, setCurrentIndex] = useState<number>(0);

    const changeImage = (index: number): void => {
        setCurrentIndex(index);
        const containerElemen: HTMLElement | null = document.querySelector('.container');
        if (containerElemen) {
            // change background
            containerElemen.style.backgroundImage = `url(${imagePath[index]})`;
            // change font-color
            const contentMarkElement: NodeListOf<HTMLElement> = document.querySelectorAll('.contHeader');
            const textColor: string = index === 1 ? 'black' : 'white';
            contentMarkElement.forEach(Link => {
                Link.style.color = textColor;
            })
            // if (contentMarkElement) {
            //   contentMarkElement.style.color = textColor;
            // }
            // change path-color
            const MenuLink: NodeListOf<HTMLAnchorElement> = document.querySelectorAll('.header a');
            const linkColor: string = index === 1 ? 'black' : 'white';
            MenuLink.forEach(link => {
                link.style.color = linkColor;
            })
            // change logo color
            const logoLink: HTMLElement | null = document.querySelector('.logo');
            if (logoLink) {
                logoLink.style.color = linkColor;
            }
            // change button color
            const slideButton: NodeListOf<HTMLAnchorElement> = document.querySelectorAll('.slideButton');
            slideButton.forEach(button => {
                button.style.color = linkColor;
            })
        }
    }
    const prevSlide = (): void => {
        const index = (currentIndex - 1 + imagePath.length) % imagePath.length;
        changeImage(index);
    }
    const nextSlide = (): void => {
        const index = (currentIndex + 1) % imagePath.length;
        changeImage(index);
    }
    return (
        <main className="container">
            <article className="content">
                <button className="slideButton prevSlide" onClick={prevSlide}><i className="fa-solid fa-chevron-left"></i></button>
                <button className="slideButton nextSlide" onClick={nextSlide}><i className="fa-solid fa-chevron-right"></i></button>
                <div className="contentMark">
                    <div className='contHeader'>
                        <h1 className="content_clain">Your Car Repairs</h1>
                        <h2 className="content_subclain">Visit us for your service</h2>
                    </div>
                    {/* <a href="#" className="book-apointment">Book Apointment</a> */}
                    <Link to={'/messages'} className="book-apointment">Book Apointment</Link>
                </div>
            </article>
        </main>
    )
}
export default Home;
