using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduViz.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    subjectId = table.Column<string>(type: "NVARCHAR(36)", nullable: false),
                    subjectName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.subjectId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    userId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    userName = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    role = table.Column<int>(type: "int", nullable: false),
                    avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    gender = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.userId);
                });

            migrationBuilder.CreateTable(
                name: "MentorDetails",
                columns: table => new
                {
                    mentorDetailsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    userId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    vipExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MentorDetails", x => x.mentorDetailsId);
                    table.ForeignKey(
                        name: "FK_MentorDetails_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    courseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    courseName = table.Column<string>(type: "NVARCHAR(200)", maxLength: 200, nullable: false),
                    mentorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    subjectId = table.Column<string>(type: "NVARCHAR(36)", nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    meetUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    picture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    startDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    duration = table.Column<int>(type: "int", nullable: false),
                    schedule = table.Column<int>(type: "int", nullable: false),
                    beginingClass = table.Column<TimeSpan>(type: "time", nullable: false),
                    endingClass = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.courseId);
                    table.ForeignKey(
                        name: "FK_Courses_MentorDetails_mentorId",
                        column: x => x.mentorId,
                        principalTable: "MentorDetails",
                        principalColumn: "mentorDetailsId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Courses_Subjects_subjectId",
                        column: x => x.subjectId,
                        principalTable: "Subjects",
                        principalColumn: "subjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MentorSubjects",
                columns: table => new
                {
                    mentorSubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    mentorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    subjectId = table.Column<string>(type: "NVARCHAR(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MentorSubjects", x => x.mentorSubjectId);
                    table.ForeignKey(
                        name: "FK_MentorSubjects_MentorDetails_mentorId",
                        column: x => x.mentorId,
                        principalTable: "MentorDetails",
                        principalColumn: "mentorDetailsId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MentorSubjects_Subjects_subjectId",
                        column: x => x.subjectId,
                        principalTable: "Subjects",
                        principalColumn: "subjectId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UpgradeOrderDetails",
                columns: table => new
                {
                    upgradeOrderDetailsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    orderCode = table.Column<long>(type: "bigint", nullable: false),
                    amount = table.Column<int>(type: "int", nullable: false),
                    packageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpgradeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    mentorDetailsID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    paymentStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpgradeOrderDetails", x => x.upgradeOrderDetailsId);
                    table.ForeignKey(
                        name: "FK_UpgradeOrderDetails_MentorDetails_mentorDetailsID",
                        column: x => x.mentorDetailsID,
                        principalTable: "MentorDetails",
                        principalColumn: "mentorDetailsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    classId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    className = table.Column<string>(type: "NVARCHAR(150)", maxLength: 150, nullable: false),
                    courseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    mentorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.classId);
                    table.ForeignKey(
                        name: "FK_Classes_Courses_courseId",
                        column: x => x.courseId,
                        principalTable: "Courses",
                        principalColumn: "courseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Classes_MentorDetails_mentorId",
                        column: x => x.mentorId,
                        principalTable: "MentorDetails",
                        principalColumn: "mentorDetailsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    paymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    studentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    mentorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    courseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    paymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    paymentStatus = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.paymentId);
                    table.ForeignKey(
                        name: "FK_Payments_Courses_courseId",
                        column: x => x.courseId,
                        principalTable: "Courses",
                        principalColumn: "courseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_MentorDetails_mentorId",
                        column: x => x.mentorId,
                        principalTable: "MentorDetails",
                        principalColumn: "mentorDetailsId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_Users_studentId",
                        column: x => x.studentId,
                        principalTable: "Users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserCourses",
                columns: table => new
                {
                    userCourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    userId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    courseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCourses", x => x.userCourseId);
                    table.ForeignKey(
                        name: "FK_UserCourses_Courses_courseId",
                        column: x => x.courseId,
                        principalTable: "Courses",
                        principalColumn: "courseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserCourses_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    postId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    content = table.Column<string>(type: "NVARCHAR", nullable: false),
                    classId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.postId);
                    table.ForeignKey(
                        name: "FK_Posts_Classes_classId",
                        column: x => x.classId,
                        principalTable: "Classes",
                        principalColumn: "classId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Quizzes",
                columns: table => new
                {
                    quizId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    quizTitle = table.Column<string>(type: "NVARCHAR(200)", maxLength: 200, nullable: false),
                    classId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    duration = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quizzes", x => x.quizId);
                    table.ForeignKey(
                        name: "FK_Quizzes_Classes_classId",
                        column: x => x.classId,
                        principalTable: "Classes",
                        principalColumn: "classId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentClasses",
                columns: table => new
                {
                    studentClassId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    userId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    classId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentClasses", x => x.studentClassId);
                    table.ForeignKey(
                        name: "FK_StudentClasses_Classes_classId",
                        column: x => x.classId,
                        principalTable: "Classes",
                        principalColumn: "classId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentClasses_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    commentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    content = table.Column<string>(type: "NVARCHAR", nullable: false),
                    postId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    userId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    parentCommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.commentId);
                    table.ForeignKey(
                        name: "FK_Comments_Comments_parentCommentId",
                        column: x => x.parentCommentId,
                        principalTable: "Comments",
                        principalColumn: "commentId");
                    table.ForeignKey(
                        name: "FK_Comments_Posts_postId",
                        column: x => x.postId,
                        principalTable: "Posts",
                        principalColumn: "postId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    questionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    questionText = table.Column<string>(type: "NVARCHAR(500)", maxLength: 500, nullable: false),
                    answerA = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    answerB = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    answerC = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: true),
                    answerD = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: true),
                    picture = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    correctAnswer = table.Column<string>(type: "NVARCHAR(1)", maxLength: 1, nullable: false),
                    quizId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.questionId);
                    table.ForeignKey(
                        name: "FK_Questions_Quizzes_quizId",
                        column: x => x.quizId,
                        principalTable: "Quizzes",
                        principalColumn: "quizId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentQuizScores",
                columns: table => new
                {
                    studentQuizScoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    userId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    quizId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    dateTaken = table.Column<DateTime>(type: "datetime2", nullable: false),
                    score = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentQuizScores", x => x.studentQuizScoreId);
                    table.ForeignKey(
                        name: "FK_StudentQuizScores_Quizzes_quizId",
                        column: x => x.quizId,
                        principalTable: "Quizzes",
                        principalColumn: "quizId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentQuizScores_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentAnswers",
                columns: table => new
                {
                    studentAnswerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    quizId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    questionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    selectedAnswer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentAnswers", x => x.studentAnswerId);
                    table.ForeignKey(
                        name: "FK_StudentAnswers_Questions_questionId",
                        column: x => x.questionId,
                        principalTable: "Questions",
                        principalColumn: "questionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentAnswers_Quizzes_quizId",
                        column: x => x.quizId,
                        principalTable: "Quizzes",
                        principalColumn: "quizId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Classes_courseId",
                table: "Classes",
                column: "courseId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_mentorId",
                table: "Classes",
                column: "mentorId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_parentCommentId",
                table: "Comments",
                column: "parentCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_postId",
                table: "Comments",
                column: "postId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_userId",
                table: "Comments",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_mentorId",
                table: "Courses",
                column: "mentorId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_subjectId",
                table: "Courses",
                column: "subjectId");

            migrationBuilder.CreateIndex(
                name: "IX_MentorDetails_userId",
                table: "MentorDetails",
                column: "userId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MentorSubjects_mentorId",
                table: "MentorSubjects",
                column: "mentorId");

            migrationBuilder.CreateIndex(
                name: "IX_MentorSubjects_subjectId",
                table: "MentorSubjects",
                column: "subjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_courseId",
                table: "Payments",
                column: "courseId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_mentorId",
                table: "Payments",
                column: "mentorId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_studentId",
                table: "Payments",
                column: "studentId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_classId",
                table: "Posts",
                column: "classId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_quizId",
                table: "Questions",
                column: "quizId");

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_classId",
                table: "Quizzes",
                column: "classId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAnswers_questionId",
                table: "StudentAnswers",
                column: "questionId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAnswers_quizId",
                table: "StudentAnswers",
                column: "quizId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentClasses_classId",
                table: "StudentClasses",
                column: "classId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentClasses_userId",
                table: "StudentClasses",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentQuizScores_quizId",
                table: "StudentQuizScores",
                column: "quizId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentQuizScores_userId",
                table: "StudentQuizScores",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_UpgradeOrderDetails_mentorDetailsID",
                table: "UpgradeOrderDetails",
                column: "mentorDetailsID");

            migrationBuilder.CreateIndex(
                name: "IX_UserCourses_courseId",
                table: "UserCourses",
                column: "courseId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCourses_userId",
                table: "UserCourses",
                column: "userId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "MentorSubjects");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "StudentAnswers");

            migrationBuilder.DropTable(
                name: "StudentClasses");

            migrationBuilder.DropTable(
                name: "StudentQuizScores");

            migrationBuilder.DropTable(
                name: "UpgradeOrderDetails");

            migrationBuilder.DropTable(
                name: "UserCourses");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Quizzes");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "MentorDetails");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
