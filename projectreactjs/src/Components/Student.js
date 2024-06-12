import axios from "axios";
import { useEffect, useState } from "react";

function Student() {
  const [studentid, setStudentID] = useState("");
  const [fName, setFullname] = useState("");
  const [age, setAge] = useState("");
  const [address, setAddress] = useState("");
  const [students, setStudents] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);

  useEffect(() => {
    getAllStudent();
  }, []);

  async function getAllStudent() {
    try {
      setIsLoading(true);
      const result = await axios.get("http://localhost:5049/api/student");
      setStudents(result.data);
    } catch (err) {
      setError("Failed to fetch students data.");
    } finally {
      setIsLoading(false);
    }
  }

  async function createStudent(event) {
    event.preventDefault();
    if (!studentid || !fName || !age || !address) {
      setError("Please fill in all fields.");
      return;
    }
    try {
      setIsLoading(true);
      await axios.post("http://localhost:5049/api/student", {
        studentID: studentid,
        fullName: fName,
        address: address,
        age: age,
      });
      alert("Create student successfully!");
      resetForm();
      getAllStudent();
    } catch (err) {
      setError("Failed to create student. Please try again.");
    } finally {
      setIsLoading(false);
    }
  }

  async function deleteStudentValue(studentID) {
    try {
      setIsLoading(true);
      await axios.delete(`http://localhost:5049/api/student/${studentID}`);
      alert("Delete student successfully!");
      getAllStudent();
    } catch (err) {
      setError("Failed to delete student. Please try again.");
    } finally {
      setIsLoading(false);
    }
  }

  async function updateStudentValue(event) {
    event.preventDefault();
    if (!studentid || !fName || !age || !address) {
      setError("Please fill in all fields.");
      return;
    }
    try {
      setIsLoading(true);
      await axios.put(`http://localhost:5049/api/student/${studentid}`, {
        studentID: studentid,
        age: age,
        address: address,
        fullName: fName,
      });
      alert("Update student successfully!");
      resetForm();
      getAllStudent();
    } catch (err) {
      setError("Failed to update student. Please try again.");
    } finally {
      setIsLoading(false);
    }
  }

  function resetForm() {
    setStudentID("");
    setFullname("");
    setAddress("");
    setAge("");
  }

  return (
    <div>
      <h1>Danh sách Student</h1>
      <hr />
      <div className="container mt-4">
        <form>
          <label>StudentID</label>
          <div className="form-group">
            <input
              type="text"
              className="form-control"
              value={studentid}
              onChange={(event) => setStudentID(event.target.value)}
            />
            <label>Student Name</label>
            <input
              type="text"
              className="form-control"
              value={fName}
              onChange={(event) => setFullname(event.target.value)}
            />
            <label>Age</label>
            <input
              type="text"
              className="form-control"
              value={age}
              onChange={(event) => setAge(event.target.value)}
            />
            <label>Address</label>
            <input
              type="text"
              className="form-control"
              value={address}
              onChange={(event) => setAddress(event.target.value)}
            />
          </div>
          <div>
            <button className="btn btn-primary mt-4" onClick={createStudent}>
              Register
            </button>
            <button
              className="btn btn-warning mt-4"
              onClick={updateStudentValue}
            >
              Update
            </button>
          </div>
        </form>
      </div>
      {isLoading ? (
        <div>Loading...</div>
      ) : error ? (
        <div>Error: {error}</div>
      ) : (
        <table className="table table-hover">
          <thead>
            <tr>
              <th scope="col">Student Id</th>
              <th scope="col">Student Name</th>
              <th scope="col">Age</th>
              <th scope="col">Address</th>
              <th scope="col">Action</th>
            </tr>
          </thead>
          <tbody>
            {students.map((std, index) => (
              <tr key={index}>
                <th scope="row">{std.studentID}</th>
                <td>{std.fullname}</td>
                <td>{std.age}</td>
                <td>{std.address}</td>
                <td>
                  <button
                    type="button"
                    className="btn btn-warning"
                    onClick={() => {
                      setStudentID(std.studentID);
                      setFullname(std.fullname);
                      setAddress(std.address);
                      setAge(std.age);
                    }}
                  >
                    Edit
                  </button>
                  <button
                    type="button"
                    className="btn btn-danger"
                    onClick={() => deleteStudentValue(std.studentID)}
                  >
                    Delete
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}

export default Student;
