using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CommentedPosts.Models;
using CommentedPosts.DTO;

namespace CommentedPosts
{
	public class MapperProfile : Profile
	{
		public MapperProfile()
		{
			CreateMap<PostDTO, Post>();
			CreateMap<CommentDTO, Comment>();
			CreateMap<Post, PostDTO>();
			CreateMap<Comment, CommentDTO>();
		}
	}
}
