using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//	http://blog.ivank.net/lightweight-matrix-class-in-c-strassen-algorithm-lu-decomposition.html
//	http://www.ivank.net/blogspot/matrix_cs/Matrix.cs
public class Matrix<T>
{
	private int rows;
	private int cols;
	private T[][] mat;

	// Constructors
	public Matrix(int rows, int cols)
	{
		this.rows = rows;
		this.cols = cols;
		Matrix<T>.ZeroMatrix(rows, cols);
	}

	//	Properties
	public int Rows { get { return rows; } }
	public int Cols { get { return cols; } }

	public T[][] Mat
	{
		get { return mat; }
		set { mat = value; }
	}

	public bool IsSquare() { return (rows == cols); }

	//	Getters/Setters
	public List<T> GetCol(int col)
	{
		List<T> arr = new List<T>(rows);

		for (int i = 0; i < rows; ++i)
			arr[i] = this.mat[i][col];

		return arr;
	}

	public List<T> GetRow(int row)
	{
		List<T> arr = new List<T>(cols);

		for (int i = 0; i < cols; ++i)
			arr[i] = this.mat[i][row];

		return arr;
	}

	public Matrix<T> Duplicate()
	{
		Matrix<T> m = new Matrix<T>(rows, cols);

		for (int i = 0; i < rows; ++i)
			for (int j = 0; j < cols; ++j)
				m.Mat[i][j] = this.mat[i][j];

		return m;
	}

	public static Matrix<T> ZeroMatrix(int r, int c)
	{
		Matrix<T> m = new Matrix<T>(r, c);

		for (int i = 0; i < r; ++i)
			for (int j = 0; j < c; ++j)
				m.Mat[i][j] = default(T);

		return m;
	}

	public static Matrix<T> Transpose(Matrix<T> m)
	{
		Matrix<T> t = new Matrix<T>(m.cols, m.rows);

		for (int i = 0; i < m.rows; i++)
			for (int j = 0; j < m.cols; j++)
				t.Mat[j][i] = m.Mat[i][j];

		return t;
	}

	public override string ToString()
	{
		System.Text.StringBuilder s = new System.Text.StringBuilder();

		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < cols; j++)
				s.Append(this.mat[i][j].ToString() + ", ");
			s.AppendLine();
		}

		return s.ToString();
	}
}

//  The class for exceptions

public class MException : System.Exception
{
	public MException(string Message) : base(Message) { }
}

