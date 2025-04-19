import React, { useState, useEffect } from 'react';
import { loginUser } from '../api/auth';
import { startTokenRefresh, getAccessToken } from '../utils/token';
import '../Auth.css';
import { useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { TOAST_OPTIONS } from '../utils/constants';
import DOMPurify from 'dompurify';

const sanitizeInput = (input) => {
  return DOMPurify.sanitize(input);
};

const Login = () => {
  const navigate = useNavigate();
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [rememberMe, setRememberMe] = useState(false);
  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const accessToken = getAccessToken();
    if (accessToken) {
      navigate('/');
    }
  }, [navigate]);

  const validateInput = (input) => {
    const regex = /^[a-zA-Z0-9@.!#$%&'*+/=?^_`{|}~-]+$/;
    return regex.test(input);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError(null);
    setLoading(true);

    const sanitizedEmail = sanitizeInput(email);
    const sanitizedPassword = sanitizeInput(password);

    if (!validateInput(sanitizedEmail) || !validateInput(sanitizedPassword)) {
      setError('Invalid input detected.');
      toast.error('Invalid input detected.', TOAST_OPTIONS);
      setLoading(false);
      return;
    }

    try {
      const loginData = { email: sanitizedEmail, password: sanitizedPassword };
      const response = await loginUser({ ...loginData, useBearerToken: true });

      toast.success(response.message || 'Login successful!', TOAST_OPTIONS);
      startTokenRefresh();

      setTimeout(() => {
        navigate('/');
      }, 500); 
    } catch (err) {
      setError(err.message || 'An error occurred');
      toast.error(err.message || 'An error occurred!', TOAST_OPTIONS);
      setLoading(false);
    }
  };

  return (
    <div className="auth-wrapper">
      <div className="auth-container">
        <form onSubmit={handleSubmit} className="auth-form">
          <h2>Login</h2>
          {error && <p className="error">{error}</p>}

          <label>Email:</label>
          <input
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />

          <label>Password:</label>
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />

          <label className="remember-me">
            <input
              type="checkbox"
              checked={rememberMe}
              onChange={(e) => setRememberMe(e.target.checked)}
            />
            <span>Remember Me</span>
          </label>

          <button type="submit" disabled={loading}>
            {loading ? (
              <div className="spinner-container">
                <div className="spinner"></div>
                <span className="loading-text">Logging in...</span>
              </div>
            ) : (
              'Login'
            )}
          </button>

          <div className="auth-toggle">
            <p>Don't have an account? <a href="/register">Register here</a></p>
          </div>
        </form>
      </div>
    </div>
  );
};

export default Login;
