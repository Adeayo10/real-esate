import React, { useState } from 'react';
import { registerUser } from '../api/auth';
import { startTokenRefresh } from '../utils/token'; 
import '../Auth.css';
import { useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { TOAST_OPTIONS } from '../utils/constants';
import { sendSMS } from '../utils/constants';



const Register = () => {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    firstName: '',
    lastName: '',
    email: '',
    password: '',
    confirmPassword: '',
    phoneNumber: '',
    address: '',
    role: 'user' 
  });
  const [error, setError] = useState(null);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError(null);

    if (formData.password !== formData.confirmPassword) {
      setError('Passwords do not match');
      toast.error('Passwords do not match', TOAST_OPTIONS);
      return;
    }

    try {
      const response = await registerUser(formData);
      console.log('Registration successful:', response);
      toast.success(response.message || "Registration successful", TOAST_OPTIONS);

      
      
      
      if (formData.phoneNumber) {
       
       console.log('Sending SMS to:', formData.phoneNumber); 
       let response = await sendSMS(formData.phoneNumber, 'Welcome to Real Estate! Your registration was successful.');
        toast.success(response.message || "SMS sent successfully", TOAST_OPTIONS);
        console.log('SMS sent successfully:', response);
      }

      startTokenRefresh(); 
      navigate('/login');
    } catch (err) {
      setError(err.message || 'An error occurred');
      toast.error(err.message || 'An error occurred', TOAST_OPTIONS);
    }
  };

  return (
    <div className="auth-wrapper">
      <div className="auth-container">
        <h2>Register</h2>
        {error && <p className="error">{error}</p>}
        <form onSubmit={handleSubmit} className="auth-form">
          <label>First Name</label>
          <input
            type="text"
            name="firstName"
            value={formData.firstName}
            onChange={handleChange}
            required
          />
          <label>Last Name</label>
          <input
            type="text"
            name="lastName"
            value={formData.lastName}
            onChange={handleChange}
            required
          />
          <label>Email</label>
          <input
            type="email"
            name="email"
            value={formData.email}
            onChange={handleChange}
            required
          />
          <label>Password</label>
          <input
            type="password"
            name="password"
            value={formData.password}
            onChange={handleChange}
            required
          />
          <label>Confirm Password</label>
          <input
            type="password"
            name="confirmPassword"
            value={formData.confirmPassword}
            onChange={handleChange}
            required
          />
          <label>Phone Number</label>
          <input
            type="tel"
            name="phoneNumber"
            value={formData.phoneNumber}
            onChange={handleChange}
            required
          />
          <label>Address</label>
          <input
            type="text"
            name="address"
            value={formData.address}
            onChange={handleChange}
            required
          />
          <button type="submit">Register</button>
          <div className="auth-toggle">
            <p>Already have an account? <a href="/login">Login here</a></p>
          </div>
        </form>
      </div>
    </div>
  );
};

export default Register;