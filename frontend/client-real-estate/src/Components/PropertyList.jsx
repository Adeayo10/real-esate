// import React, { useEffect, useState } from 'react';
// import './PropertyList.css';
// import axios from 'axios';

// const PropertyList = () => {
//     const [properties, setProperties] = useState([]);
//     const [filteredProperties, setFilteredProperties] = useState([]);
//     const [sortOption, setSortOption] = useState('price');
//     const [sortOrder, setSortOrder] = useState('asc');
//     const [searchTerm, setSearchTerm] = useState('');
//     const [selectedType, setSelectedType] = useState('all');
//     const [pageNumber, setPageNumber] = useState(1);
//     const [pageSize] = useState(6); // Customize how many properties per page
//     const [totalPages, setTotalPages] = useState(1);

//     const fetchProperties = async () => {
//         try {
//             let response;
//             if (searchTerm.trim() === '') {
//                 response = await axios.get('/api/list', {
//                     params: { pageNumber, pageSize }
//                 });
//             } else {
//                 response = await axios.get('/api/list/search', {
//                     params: { search: searchTerm }
//                 });
//             }

//             let data = response.data.data || response.data;
//             setProperties(data);

//             // Apply type filter
//             if (selectedType !== 'all') {
//                 data = data.filter(p => p.type?.toLowerCase() === selectedType);
//             }

//             setFilteredProperties(data);

//             const total = data.length;
//             setTotalPages(Math.ceil(total / pageSize));
//         } catch (error) {
//             console.error("Failed to fetch properties", error);
//         }
//     };

//     useEffect(() => {
//         fetchProperties();
//     }, [pageNumber, searchTerm, selectedType]);

//     const handleSortChange = (e) => {
//         const option = e.target.value;
//         const newSortOrder = sortOrder === 'asc' ? 'desc' : 'asc';
//         setSortOption(option);
//         setSortOrder(newSortOrder);
//         sortProperties(option, newSortOrder);
//     };

//     const sortProperties = (option, order) => {
//         let sorted = [...filteredProperties];
//         if (option === 'price') {
//             sorted.sort((a, b) => (order === 'asc' ? a.price - b.price : b.price - a.price));
//         } else if (option === 'type') {
//             sorted.sort((a, b) => a.type.localeCompare(b.type));
//         }
//         setFilteredProperties(sorted);
//     };

//     const handleSearch = (e) => {
//         setSearchTerm(e.target.value.toLowerCase());
//         setPageNumber(1); // Reset to page 1 on new search
//     };

//     const handleTypeChange = (e) => {
//         setSelectedType(e.target.value);
//         setPageNumber(1); // Reset to page 1 on type change
//     };

//     const handlePageChange = (direction) => {
//         setPageNumber(prev =>
//             direction === 'next' ? Math.min(prev + 1, totalPages) : Math.max(prev - 1, 1)
//         );
//     };

//     // For displaying only the properties on the current page
//     const paginatedProperties = filteredProperties.slice(
//         (pageNumber - 1) * pageSize,
//         pageNumber * pageSize
//     );

//     return (
//         <div className="property-list-container">
//             <div className="top-controls">
//                 {/* Sorting UI */}
//                 <div className="sort-option">
//                     <h4>Sort By</h4>
//                     <div className="sort-option">
//                         <label>Price Low to High</label>
//                         <input
//                             type="radio"
//                             value="price"
//                             checked={sortOption === 'price' && sortOrder === 'asc'}
//                             onChange={handleSortChange}
//                         />
//                     </div>
//                     <div className="sort-option">
//                         <label>Price High to Low</label>
//                         <input
//                             type="radio"
//                             value="price"
//                             checked={sortOption === 'price' && sortOrder === 'desc'}
//                             onChange={handleSortChange}
//                         />
//                     </div>
//                 </div>

//                 {/* Filter UI */}
//                 <div className="filter-option">
//                     <h4>Filter by Type</h4>
//                     <select onChange={handleTypeChange} value={selectedType}>
//                         <option value="all">All Types</option>
//                         <option value="apartment">Apartment</option>
//                         <option value="villa">Villa</option>
//                         <option value="house">House</option>
//                         <option value="townhouse">Townhouse</option>
//                     </select>
//                 </div>

//                 {/* Search UI */}
//                 <div className="search-bar2">
//                     <h4>Search</h4>
//                     <input
//                         type="text"
//                         placeholder="Search by address, price, or type"
//                         value={searchTerm}
//                         onChange={handleSearch}
//                     />
//                 </div>
//             </div>

//             <div className="property-cards">
//                 {paginatedProperties.map((property) => (
//                     <div className="property-card" key={property.id}>
//                         <img
//                             src={"https://wallpaperaccess.com/full/1408420.jpg"}
//                             alt={property.name}
//                         />
//                         <div className="property-details">
//                             <h4>{property.name}</h4>
//                             <p>Price: ${property.price}</p>
//                             <p>Location: {property.location}</p>
//                             <p>Type: {property.type}</p>
//                         </div>
//                     </div>
//                 ))}
//             </div>

//             <div className="pagination">
//                 <button onClick={() => handlePageChange('prev')} disabled={pageNumber === 1}>
//                     Prev
//                 </button>
//                 <span>Page {pageNumber} of {totalPages}</span>
//                 <button onClick={() => handlePageChange('next')} disabled={pageNumber === totalPages}>
//                     Next
//                 </button>
//             </div>
//         </div>
//     );
// };

// export default PropertyList;
