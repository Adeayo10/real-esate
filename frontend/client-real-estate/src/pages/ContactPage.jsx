import React, { useState } from "react";
import './contactPage.css'; 
import { useNavigate } from 'react-router-dom';  
import { createContactUs } from "../api/contact";  

function ContactPage() {
  const [formData, setFormData] = useState({
    firstName: "",
    lastName: "",
    email: "",
    phoneNumber: "",
    message: "",
    address: ""  
  });

  const [successMessage, setSuccessMessage] = useState("");
  const [errorMessage, setErrorMessage] = useState("");
  const [showModal, setShowModal] = useState(false);  

  const navigate = useNavigate();  

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    const fullName = `${formData.firstName} ${formData.lastName}`;

    const contactData = {
      name: fullName,
      email: formData.email,
      phoneNumber: formData.phoneNumber,
      message: formData.message,
      address: formData.address  
    };

    try {
      const response = await createContactUs(contactData);  
      console.log("Submitted:", response);
      setSuccessMessage("Thanks for contacting us!");
      setErrorMessage("");
      setFormData({
        firstName: "",
        lastName: "",
        email: "",
        phoneNumber: "",
        message: "",
        address: "" 
      });
      setShowModal(true);  
    } catch (error) {
      console.error("Error submitting form:", error);
      setErrorMessage("There was an error. Please try again later.");
      setSuccessMessage("");
    }
  };


  const closeModal = () => {
    setShowModal(false);
    navigate('/');  
  };

  return (
    <div className="contact-form-container">
      <h1>Contact Us</h1>
      <p>We'd love to hear from you! Please fill out the form below.</p>

      {successMessage && <p className="success">{successMessage}</p>}
      {errorMessage && <p className="error">{errorMessage}</p>}

      <form onSubmit={handleSubmit}>
        <div>
          <label htmlFor="firstName">First Name</label>
          <input
            type="text"
            id="firstName"
            name="firstName"
            value={formData.firstName}
            onChange={handleChange}
            required
          />
        </div>

        <div>
          <label htmlFor="lastName">Last Name</label>
          <input
            type="text"
            id="lastName"
            name="lastName"
            value={formData.lastName}
            onChange={handleChange}
            required
          />
        </div>

        <div>
          <label htmlFor="email">Email</label>
          <input
            type="email"
            id="email"
            name="email"
            value={formData.email}
            onChange={handleChange}
            required
          />
        </div>

        <div>
          <label htmlFor="phoneNumber">Phone Number</label>
          <input
            type="text"
            id="phoneNumber"
            name="phoneNumber"
            value={formData.phoneNumber}
            onChange={handleChange}
            required
          />
        </div>

        <div>
          <label htmlFor="address">Address</label>
          <input
            type="text"
            id="address"
            name="address"
            value={formData.address}
            onChange={handleChange}
            required  
          />
        </div>

        <div>
          <label htmlFor="message">Message</label>
          <textarea
            id="message"
            name="message"
            value={formData.message}
            onChange={handleChange}
            required
          ></textarea>
        </div>

        <button type="submit">Submit</button>
      </form>
      {showModal && (
        <div className="modal">
          <div className="modal-content">
            <h2>Thanks for reaching out to us!</h2>
            <button onClick={closeModal}>Close</button>
          </div>
        </div>
      )}
    </div>
  );
}

export default ContactPage;
