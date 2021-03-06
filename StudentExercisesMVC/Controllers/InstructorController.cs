﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentExercisesMVC.Models;
using StudentExercisesMVC.Models.ViewModels;

namespace StudentExercisesMVC.Controllers
{
    public class InstructorController : Controller
    {
        private readonly IConfiguration _config;

        public InstructorController(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }
        // GET: Instructor
        public ActionResult Index()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"select i.id, i.FirstName, i.LastName, i.SlackHandle, i.CohortId, cohort.[Name] as cohortName
                                        from Instructors i inner join Cohort on i.CohortId = Cohort.Id";
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Instructor> instructorList = new List<Instructor>();
                    while (reader.Read())
                    {
                        Instructor instructor = new Instructor
                        {
                            id = reader.GetInt32(reader.GetOrdinal("id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            slackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                            CohortName = reader.GetString(reader.GetOrdinal("cohortName"))
                        };
                        instructorList.Add(instructor);
                    }
                    reader.Close();
                    return View(instructorList);
                }
            }
        }

        // GET: Instructor/Details/5
        public ActionResult Details(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $@"select i.id, i.FirstName, i.LastName, i.SlackHandle, i.CohortId, cohort.[Name] as cohortName
                                        from Instructors i inner join Cohort on i.CohortId = Cohort.Id
                                        Where i.id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();

                    Instructor instructor = null;
                    if (reader.Read())
                    {
                        instructor = new Instructor
                        {
                            id = reader.GetInt32(reader.GetOrdinal("id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            slackHandle = reader.GetString(reader.GetOrdinal("slackHandle")),
                            CohortName = reader.GetString(reader.GetOrdinal("cohortName"))
                        };

                    }
                    reader.Close();
                    return View(instructor);

                }
            }
        }

        // GET: Instructor/Create
        public ActionResult Create()
        {
            InstructorCreateViewModel viewModel =
            new InstructorCreateViewModel(_config.GetConnectionString("DefaultConnection"));
            return View(viewModel);
        }

        // POST: Instructor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InstructorCreateViewModel viewModel)
        {
            try
            {
                // TODO: Add insert logic here
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"INSERT INTO Instructors (FirstName, LastName, SlackHandle, CohortId)
                                            Values (@FirstName, @LastName, @SlackHandle, @CohortId)";
                        cmd.Parameters.Add(new SqlParameter("@FirstName", viewModel.instructor.FirstName));
                        cmd.Parameters.Add(new SqlParameter("@LastName", viewModel.instructor.LastName));
                        cmd.Parameters.Add(new SqlParameter("@SlackHandle", viewModel.instructor.slackHandle));
                        cmd.Parameters.Add(new SqlParameter("@CohortId", viewModel.instructor.CohortId));

                        cmd.ExecuteNonQuery();

                        return RedirectToAction(nameof(Index));
                    }
                }

            }
            catch
            {
                viewModel.Cohorts = GetAllCohorts();
                return View(viewModel);
            }
        }

        // GET: Instructor/Edit/5
        public ActionResult Edit(int id)
        {
            Instructor instructor = GetInstructorById(id);
            if(instructor == null)
            {
                return NotFound();
            }
            InstructorEditViewModel viewModel = new InstructorEditViewModel
            {
                Cohorts = GetAllCohorts(),
                Instructor = instructor
            };

            return View(viewModel);
        }

        // POST: Instructor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, InstructorEditViewModel viewModel)
        {
            try
            {
                // TODO: Add update logic here
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using(SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE Instructors 
                                            SET FirstName = @FirstName,
                                                LastName = @LastName,
                                                SlackHandle = @SlackHandle,
                                                CohortId = @CohortId
                                            Where id = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", id));
                        cmd.Parameters.Add(new SqlParameter("@FirstName", viewModel.Instructor.FirstName));
                        cmd.Parameters.Add(new SqlParameter("@LastName", viewModel.Instructor.LastName));
                        cmd.Parameters.Add(new SqlParameter("@SlackHandle", viewModel.Instructor.slackHandle));
                        cmd.Parameters.Add(new SqlParameter("@CohortId", viewModel.Instructor.CohortId));

                        cmd.ExecuteNonQuery();

                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch
            {
                viewModel.Cohorts = GetAllCohorts();
                return View(viewModel);
            }
        }

        // GET: Instructor/Delete/5
        public ActionResult Delete(int id)
        {
            Instructor instructor = GetInstructorById(id);
            if (instructor == null)
            {
                return NotFound();
            }
            else
            {
                return View(instructor);
            }

            
        }

        // POST: Instructor/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Instructor instructor)
        {
                // TODO: Add delete logic here
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"Delete from Instructors where id = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        cmd.ExecuteNonQuery();
                        return RedirectToAction(nameof(Index));
                    }
                }

        }
        private Instructor GetInstructorById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT i.Id AS InstructorId,
                                               i.FirstName, i.LastName, 
                                               i.SlackHandle, i.CohortId,
                                               c.Name AS CohortName
                                          FROM Instructors i LEFT JOIN Cohort c on i.cohortid = c.id
                                         WHERE  i.Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Instructor instructor = null;

                    if (reader.Read())
                    {
                        instructor = new Instructor
                        {
                            id = reader.GetInt32(reader.GetOrdinal("InstructorId")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            slackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("CohortId")),
                            Cohort = new Cohort
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("CohortId")),
                                cohortName = reader.GetString(reader.GetOrdinal("CohortName")),
                            }
                        };
                    }

                    reader.Close();

                    return instructor;
                }
            }

        }


        private List<Cohort> GetAllCohorts()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT id, [name] as cohortName from Cohort;";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Cohort> cohorts = new List<Cohort>();

                    while (reader.Read())
                    {
                        cohorts.Add(new Cohort
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            cohortName = reader.GetString(reader.GetOrdinal("cohortName"))
                        });
                    }
                    reader.Close();

                    return cohorts;
                }
            }

        }
    }
}