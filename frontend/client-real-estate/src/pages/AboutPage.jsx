import React from "react";
import './about.css';  // Ensure this path is correct

function AboutUsPage() {
  return (
    <div className="about-us-container">
      <header className="about-header">
        <h1>About Us</h1>
        <p>We are transforming the real estate experience with innovation, transparency, and efficiency.</p>
      </header>

      <section className="about-section">
        <h2>Our Mission</h2>
        <p>
          Our mission is to revolutionize real estate management by offering cutting-edge technology, empowering clients with seamless and reliable solutions for property transactions and management.
        </p>
      </section>

      <section className="about-section">
        <h2>Our Vision</h2>
        <p>
          Our vision is to become the world's leading platform for real estate solutions, known for our innovation, customer service, and transparency. We aim to make real estate accessible and manageable for everyone.
        </p>
      </section>

      <section className="team-section">
        <h2>Meet Our Team</h2>
        <div className="team-members">
          <div className="team-member">
            
            <h3>Ayo</h3>
            <p>CEO & Founder</p>
            <p>With over 15 years of experience in real estate, Ayo is the visionary leader behind the company's growth and success.</p>
          </div>
          <div className="team-member">
         
            <h3>Mohammed Javeed</h3>
            <p>COO</p>
            <p>Javeed manages operations and ensures smooth processes across all departments, leading to enhanced customer satisfaction.</p>
          </div>
          <div className="team-member">
            
            <h3>Nibu George</h3>
            <p>Lead Developer</p>
            <p>Nibu is the mastermind behind our platform's architecture and ensures the scalability and security of our systems.</p>
          </div>
          <div className="team-member">
           
            <h3>Bincy</h3>
            <p>Software Architect</p>
            <p>Bincy is the visionary behind our software architecture, crafting robust and scalable solutions that drive performance, reliability, and future growth.</p>
          </div>
        </div>
      </section>

      <section className="achievements-section">
        <h2>Our Achievements</h2>
        <div className="achievements-list">
          <div className="achievement">
            <h3>500+ Projects Completed</h3>
            <p>Successfully delivering over 500 projects, we have become a trusted partner for businesses and individuals in the real estate space.</p>
          </div>
          <div className="achievement">
            <h3>10+ Years of Expertise</h3>
            <p>Our decade-long experience allows us to offer solutions that are not only innovative but also reliable and effective in the real estate market.</p>
          </div>
          <div className="achievement">
            <h3>Award-Winning Platform</h3>
            <p>Our platform has received multiple industry awards for its design, functionality, and user-centered approach.</p>
          </div>
        </div>
      </section>

      <footer className="about-footer">
        <p>&copy; 2025 Real Estate Solutions. All rights reserved.</p>
      </footer>
    </div>
  );
}

export default AboutUsPage;
