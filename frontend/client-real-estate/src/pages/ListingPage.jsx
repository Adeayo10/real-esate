import React, { useEffect, useState } from 'react';
import '../Components/PropertyList.css';
import axios from 'axios';
import { useLocation } from 'react-router-dom'; 

const PropertyList = () => {
    const [properties, setProperties] = useState([]);
    const [filteredProperties, setFilteredProperties] = useState([]);
    const [sortOption, setSortOption] = useState('price');
    const [sortOrder, setSortOrder] = useState('asc');
    const [searchTerm, setSearchTerm] = useState('');
    const [pageNumber, setPageNumber] = useState(1);
    const [pageSize] = useState(8);
    const [totalPages, setTotalPages] = useState(1);

    const location = useLocation(); 

    const fetchProperties = async () => {
        try {
            let response;

            if (searchTerm.trim() !== '') {
                console.log("Fetching by search term:", searchTerm);
                response = await axios.get('/api/list/search', {
                    params: { search: searchTerm }
                });
            } else {
                console.log("Fetching paginated list");
                response = await axios.get('/api/list', {
                    params: { pageNumber, pageSize }
                });
            }

            let data = response.data.data || response.data;
            console.log("Raw API Data:", data);

            data = data.map(property => ({
                ...property,
                price: Number(property.price)
            }));

            sortProperties(data);

            setProperties(data);
            setFilteredProperties(data);

            const total = data.length;
            setTotalPages(Math.ceil(total / pageSize));
        } catch (error) {
            console.error("Failed to fetch properties", error);
        }
    };

    useEffect(() => {
        if (location.state && location.state.filteredProperties) {
            setFilteredProperties(location.state.filteredProperties);
        } else {
            fetchProperties();
        }
    }, [pageNumber, searchTerm, location.state]);

    const handleSortChange = (e) => {
        const option = e.target.value;
        const newSortOrder = sortOrder === 'asc' ? 'desc' : 'asc';
        setSortOption(option);
        setSortOrder(newSortOrder);
        sortProperties(filteredProperties, option, newSortOrder);
    };

    const sortProperties = (data, option = sortOption, order = sortOrder) => {
        let sorted = [...data];
        if (option === 'price') {
            sorted.sort((a, b) =>
                order === 'asc' ? a.price - b.price : b.price - a.price
            );
        } else if (option === 'type') {
            sorted.sort((a, b) => a.type.localeCompare(b.type));
        }
        setFilteredProperties(sorted);
    };

    const handleSearch = (e) => {
        setSearchTerm(e.target.value.toLowerCase());
        setPageNumber(1);
    };

    const handlePageChange = (direction) => {
        setPageNumber(prev =>
            direction === 'next' ? Math.min(prev + 1, totalPages) : Math.max(prev - 1, 1)
        );
    };

    const paginatedProperties = filteredProperties.slice(
        (pageNumber - 1) * pageSize,
        pageNumber * pageSize
    );

    return (
        <div className="property-list-container">
            <div className="top-controls">
            <div className="sort-option">
    <h4>Sort By Price</h4>
    <div style={{ display: 'flex', gap: '15px' }}>
        <label style={{ display: 'flex', alignItems: 'center', gap: '5px' }}>
            <input
                type="radio"
                value="price"
                checked={sortOption === 'price' && sortOrder === 'asc'}
                onChange={handleSortChange}
            />
            Low to High
        </label>
        <label style={{ display: 'flex', alignItems: 'center', gap: '5px' }}>
            <input
                type="radio"
                value="price"
                checked={sortOption === 'price' && sortOrder === 'desc'}
                onChange={handleSortChange}
            />
            High to Low
        </label>
    </div>
</div>

                <div className="search-bar">
                    <h4>Search</h4>
                    <input
                        type="text"
                        placeholder="Search by address, price, or type"
                        value={searchTerm}
                        onChange={handleSearch}
                    />
                </div>
            </div>

            <div className="property-cards">
                {paginatedProperties.map((property) => (
                    <div className="property-card" key={property.id} style={{ background: '#f8f9fa', padding: '15px', borderRadius: '8px' }}>
                        <img
                            src={"https://wallpaperaccess.com/full/1408420.jpg"}
                            alt={property.name}
                            style={{ width: '100%', height: '200px', objectFit: 'cover', borderRadius: '8px' }}
                        />
                        <div className="property-details" style={{ marginTop: '10px' }}>
                            <h4 style={{color:'black'}}>{property.name}</h4>
                            <p><strong>Price:</strong> ${property.price}</p>
                            <p><strong>Location:</strong> {property.address}</p>
                            <p><strong>House Type:</strong> {property.houseType}</p>
                            <p><strong>Type:</strong> {property.mode}</p>
                        </div>
                    </div>
                ))}
            </div>

            <div className="pagination" style={{ marginTop: '20px' }}>
                <button onClick={() => handlePageChange('prev')} disabled={pageNumber === 1}>
                    Prev
                </button>
                <span>Page {pageNumber} of {totalPages}</span>
                <button onClick={() => handlePageChange('next')} disabled={pageNumber === totalPages}>
                    Next
                </button>
            </div>
        </div>
    );
};

export default PropertyList;
