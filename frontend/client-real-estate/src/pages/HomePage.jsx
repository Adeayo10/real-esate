import React from 'react';

function HomePage() {
  return (
    <div className="landing-container">
      <nav className="navbar">
        <h1 className="logo">PROPERTY L!STING</h1>
        <ul className="nav-links">
          <li><a href="/listings">Listings</a></li>
          <li><a href="/about">About</a></li>
          <li><a href="/contact">Contact</a></li>
        </ul>
      </nav>
      
      <header className="hero">
        <div className="hero-content">
          <h1>Welcome to Our Website</h1>
          <p>Your one-stop destination for amazing experiences.</p>
          <a href="/listings" className="cta-button">Explore Listings</a>
        </div>
      </header>
      
      <section className="features">
        <div className="feature">
          <h2>Quality Services</h2>
          <p>We provide top-notch services that you can rely on.</p>
        </div>
        <div className="feature">
          <h2>Trusted by Thousands</h2>
          <p>Join a community of happy customers.</p>
        </div>
        <div className="feature">
          <h2>24/7 Support</h2>
          <p>We are here to assist you anytime.</p>
        </div>
      </section>
    </div>
  );
}

export default HomePage;
