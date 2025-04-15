import React, { useState } from 'react';
import './PropertyList.css';
import { Link } from "react-router-dom";
import property1 from '../assets/property1.jpg';
import property2 from '../assets/property2.jpg';
import property3 from '../assets/property3.jpg';

// Dummy data for properties
const properties = [
    { id: 1, name: "3 BHK Apartment in Dublin", price: 250000, location: "Dublin", type: "Apartment", image: property1 },
    { id: 2, name: "1 BHK Apartment in Cork", price: 100000, location: "Cork", type: "Apartment", image: property2 },
    { id: 3, name: "2 BHK Apartment in Galway", price: 150000, location: "Galway", type: "Apartment", image: property3 },
    { id: 4, name: "3 BHK Villa in Limerick", price: 300000, location: "Limerick", type: "Villa", image: property1 },
    { id: 5, name: "1 BHK Villa in Kilkenny", price: 120000, location: "Kilkenny", type: "Villa", image: property2 },
    { id: 6, name: "2 BHK Apartment in Cork", price: 175000, location: "Cork", type: "Apartment", image: property3 },
    { id: 7, name: "4 BHK House in Dublin", price: 450000, location: "Dublin", type: "House", image: property1 },
    { id: 8, name: "2 BHK Apartment in Cork", price: 160000, location: "Cork", type: "Apartment", image: property2 },
    { id: 9, name: "3 BHK Townhouse in Galway", price: 220000, location: "Galway", type: "Townhouse", image: property3 },
    { id: 10, name: "1 BHK Apartment in Dublin", price: 120000, location: "Dublin", type: "Apartment", image: property1 },
    { id: 11, name: "2 BHK House in Cork", price: 200000, location: "Cork", type: "House", image: property2 },
    { id: 12, name: "3 BHK Villa in Kilkenny", price: 350000, location: "Kilkenny", type: "Villa", image: property3 },
    { id: 13, name: "2 BHK Townhouse in Limerick", price: 180000, location: "Limerick", type: "Townhouse", image: property1 },
    { id: 14, name: "1 BHK Apartment in Galway", price: 90000, location: "Galway", type: "Apartment", image: property2 },
    { id: 15, name: "3 BHK House in Kilkenny", price: 320000, location: "Kilkenny", type: "House", image: property3 }
];

const PropertyList = () => {
    const [filteredProperties, setFilteredProperties] = useState(properties);
    const [sortOption, setSortOption] = useState('price');
    const [sortOrder, setSortOrder] = useState('asc');
    const [searchTerm, setSearchTerm] = useState('');
    const [selectedType, setSelectedType] = useState('all');

    const handleSortChange = (e) => {
        const option = e.target.value;
        setSortOption(option);
        let newSortOrder = sortOrder;
        if (option === 'price') {
            newSortOrder = sortOrder === 'asc' ? 'desc' : 'asc';
        }

        setSortOrder(newSortOrder);  
        sortProperties(option, newSortOrder); 
    };

    const sortProperties = (option, order) => {
        let sortedProperties = [...filteredProperties];
        if (option === 'price') {
            sortedProperties = sortedProperties.sort((a, b) => (order === 'asc' ? a.price - b.price : b.price - a.price));
        } else if (option === 'type') {
            sortedProperties = sortedProperties.sort((a, b) => a.type.localeCompare(b.type));
        }

        setFilteredProperties(sortedProperties); 
    };

    const handleSearch = (e) => {
        const term = e.target.value.toLowerCase();
        setSearchTerm(term);
        
        const filtered = properties.filter(property => {
            return (
                property.name.toLowerCase().includes(term) ||
                property.price.toString().includes(term) ||
                property.type.toLowerCase().includes(term)
            );
        });

        if (selectedType !== 'all') {
            setFilteredProperties(filtered.filter(property => property.type.toLowerCase() === selectedType));
        } else {
            setFilteredProperties(filtered);
        }
    };

    const handleTypeChange = (e) => {
        const type = e.target.value;
        setSelectedType(type);

        const filtered = properties.filter(property => {
            return (
                property.name.toLowerCase().includes(searchTerm) ||
                property.price.toString().includes(searchTerm) ||
                property.type.toLowerCase().includes(searchTerm)
            );
        });

        if (type !== 'all') {
            setFilteredProperties(filtered.filter(property => property.type.toLowerCase() === type));
        } else {
            setFilteredProperties(filtered);
        }
    };

    return (
        <div className="property-list-container">
            <div className="sidebar">
                <h3>Sort By</h3>
                <div className="sort-option">
                    <label>Price Low to High</label>
                    <input
                        type="radio"
                        value="price"
                        checked={sortOption === 'price' && sortOrder === 'asc'}
                        onChange={handleSortChange}
                    />
                </div>
                <div className="sort-option">
                    <label>Price High to Low</label>
                    <input
                        type="radio"
                        value="price"
                        checked={sortOption === 'price' && sortOrder === 'desc'}
                        onChange={handleSortChange}
                    />
                </div>

                <h4>Filter by Type</h4>
                <select onChange={handleTypeChange} value={selectedType}>
                    <option value="all">All Types</option>
                    <option value="apartment">Apartment</option>
                    <option value="villa">Villa</option>
                    <option value="house">House</option>
                    <option value="townhouse">Townhouse</option>
                </select>

                <div className="search-bar2">
                    <input
                        type="text"
                        placeholder="Search by address, price, or type"
                        value={searchTerm}
                        onChange={handleSearch}
                    />
                </div>
            </div>

            <div className="property-cards">
                {filteredProperties.map((property) => (
                    <div className="property-card" key={property.id}>
                        <img src={property.image} alt={property.name} />
                        <div className="property-details">
                            <h4>{property.name}</h4>
                            <p>Price: ${property.price}</p>
                            <p>Location: {property.location}</p>
                            <p>Type: {property.type}</p>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default PropertyList;
